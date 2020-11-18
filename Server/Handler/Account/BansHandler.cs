using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Account
{
    public class BansHandler : DatabaseEntityWrapper
    {
        public bool LoadedServerBans { get; private set; }

        private readonly EventsHandler _eventsHandler;

        private readonly LobbiesHandler _lobbiesHandler;

        private readonly ISettingsHandler _settingsHandler;
        private readonly ILoggingHandler _logger;

        private List<PlayerBans> _cachedBans = new List<PlayerBans>();

        public BansHandler(TDSDbContext dbContext, LobbiesHandler lobbiesHandler, EventsHandler eventsHandler,
            ISettingsHandler settingsHandler, ILoggingHandler logger)
            : base(dbContext)
        {
            _lobbiesHandler = lobbiesHandler;
            _settingsHandler = settingsHandler;
            _eventsHandler = eventsHandler;
            _logger = logger;

            eventsHandler.Hour += RemoveExpiredBans;
            eventsHandler.Minute += RefreshServerBansCache;

            eventsHandler.IncomingConnection += CheckBanOnIncomingConnection;
        }

        public void AddServerBan(PlayerBans ban)
        {
            try
            {
                RemoveServerBan(ban);
                lock (_cachedBans)
                {
                    _cachedBans.Add(ban);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        public void RemoveServerBan(PlayerBans oldBan)
        {
            try
            {
                lock (_cachedBans)
                {
                    var ban = _cachedBans.FirstOrDefault(b => b.PlayerId == oldBan.PlayerId && b.LobbyId == oldBan.LobbyId);
                    _cachedBans.Remove(oldBan);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        public async Task<PlayerBans?> GetBan(int lobbyId,
                    int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            try
            {
                PlayerBans? ban = (playerId, ip, serial, socialClubName, socialClubId, andConnection) switch
                {
                    ({ }, null, null, null, null, _)
                        => await ExecuteForDBAsync(async (dbContext) =>
                        {
                            return await dbContext.PlayerBans.WherePlayerAndLobby(playerId!.Value, lobbyId).FirstOrDefaultAsync().ConfigureAwait(false);
                        }).ConfigureAwait(false),

                    (_, _, _, _, _, true)
                         => await ExecuteForDBAsync(async (dbContext) =>
                         {
                             return await dbContext.PlayerBans
                                .WhereAllConditions(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);
                         }).ConfigureAwait(false),

                    (_, _, _, _, _, false)
                         => await ExecuteForDBAsync(async (dbContext) =>
                         {
                             return await dbContext.PlayerBans
                                .WhereOneCondition(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);
                         }).ConfigureAwait(false),
                };

                if (ban is null)
                    return null;

                if (RemoveBanIfExpired(ban))
                    return null;

                return ban;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        public PlayerBans? GetServerBan(int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            try
            {
                int lobbyId = _lobbiesHandler.MainMenu.Entity.Id;
                PlayerBans? ban;
                lock (_cachedBans)
                {
                    ban = (playerId, ip, serial, socialClubName, socialClubId, andConnection) switch
                    {
                        ({ }, null, null, null, null, _)
                            => _cachedBans.AsQueryable().WherePlayerAndLobby(playerId!.Value, lobbyId).FirstOrDefault(),

                        (_, _, _, _, _, true)
                             => _cachedBans.AsQueryable().WhereAllConditions(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection).FirstOrDefault(),

                        (_, _, _, _, _, false)
                             => _cachedBans.AsQueryable().WhereOneCondition(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection).FirstOrDefault()
                    };
                }

                if (ban is null)
                    return null;

                if (RemoveBanIfExpired(ban))
                {
                    lock (_cachedBans) 
                        _cachedBans.Remove(ban);
                    return null;
                }
                    

                return ban;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        public async void RefreshServerBansCache(int counter)
        {
            try
            {
                if (counter % _settingsHandler.ServerSettings.ReloadServerBansEveryMinutes != 0)
                    return;

                int lobbyId = _lobbiesHandler.MainMenu.Entity.Id;
                var entries = await ExecuteForDBAsync(async dbContext
                    => await dbContext.PlayerBans.Where(b => b.LobbyId == lobbyId).Include(b => b.Admin).ToListAsync().ConfigureAwait(false)).ConfigureAwait(false);
                
                if (entries is null)
                    return;
                lock (_cachedBans)
                {
                    _cachedBans = entries;
                }

                LoadedServerBans = true;
                _eventsHandler.OnLoadedServerBans();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void RemoveServerBanByPlayerId(PlayerBans ban)
        {
            lock (_cachedBans)
            {
                var banToRemove = _cachedBans.FirstOrDefault(b => b.LobbyId == ban.LobbyId && b.PlayerId == ban.PlayerId);
                if (banToRemove is { })
                    _cachedBans.Remove(banToRemove);
            }
        }

        private void CheckBanOnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            var ban = GetServerBan(null, ip, serial, socialClubName, socialClubId, true);
            if (ban is { })
                cancel.Cancel = true;
        }

        private async void RemoveExpiredBans(int _)
        {
            try
            {
                await ExecuteForDBAsync(async (dbContext) =>
                {
                    var bans = await dbContext.PlayerBans
                        .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.UtcNow)
                        .ToListAsync()
                        .ConfigureAwait(false);
                    dbContext.PlayerBans.RemoveRange(bans);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private bool RemoveBanIfExpired(PlayerBans ban)
        {
            if (ban.EndTimestamp <= DateTime.UtcNow)
            {
                ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Entry(ban).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }).IgnoreResult();
                return true;
            }
            return false;
        }
    }
}
