﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.Account
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
                            return await dbContext.PlayerBans.FirstOrDefaultAsync(ban => 
                                GetConditionForPlayerAndLobby(playerId!.Value, lobbyId)(ban));
                        }),

                    (_, _, _, _, _, true)
                         => await ExecuteForDBAsync(async (dbContext) =>
                         {
                             return await dbContext.PlayerBans
                                .FirstOrDefaultAsync(ban => 
                                    GetConditionForSatisfyingAllConditions(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection)(ban));
                         }),

                    (_, _, _, _, _, false)
                         => await ExecuteForDBAsync(async (dbContext) =>
                         {
                             return await dbContext.PlayerBans
                                .FirstOrDefaultAsync(ban => 
                                    GetConditionBanSatisfyingOneCondition(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection)(ban));
                         }),
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
                            => _cachedBans.FirstOrDefault(GetConditionForPlayerAndLobby(playerId!.Value, lobbyId)),

                        (_, _, _, _, _, true)
                             => _cachedBans.FirstOrDefault(GetConditionForSatisfyingAllConditions(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection)),

                        (_, _, _, _, _, false)
                             => _cachedBans.FirstOrDefault(GetConditionBanSatisfyingOneCondition(lobbyId, playerId, ip, serial, socialClubName, socialClubId, preventConnection))
                    };
                }

                if (ban is null)
                    return null;

                if (RemoveBanIfExpired(ban))
                {
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
                    => await dbContext.PlayerBans.Where(b => b.LobbyId == lobbyId).Include(b => b.Admin).ToListAsync());
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
                        .ToListAsync();
                    dbContext.PlayerBans.RemoveRange(bans);
                    await dbContext.SaveChangesAsync();
                });
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
                    await dbContext.SaveChangesAsync();
                }).IgnoreResult();
                return true;
            }
            return false;
        }

        private Func<PlayerBans, bool> GetConditionForPlayerAndLobby(int playerId, int lobbyId)
            => ban => ban.PlayerId == playerId && ban.LobbyId == lobbyId;

        private Func<PlayerBans, bool> GetConditionForSatisfyingAllConditions(int lobbyId,
                   int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
                   bool? preventConnection = null)
            => b => b.LobbyId == lobbyId
                        && (playerId == null || b.PlayerId == playerId)
                        && (ip == null || b.IP == ip)
                        && (serial == null || b.Serial == serial)
                        && (socialClubName == null || b.SCName == socialClubName)
                        && (socialClubId == null || b.SCId == socialClubId)
                        && (preventConnection == null || b.PreventConnection == preventConnection);

        private Func<PlayerBans, bool> GetConditionBanSatisfyingOneCondition(int lobbyId,
                  int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
                  bool? preventConnection = null)
          => b => b.LobbyId == lobbyId && (
                      (playerId == null || b.PlayerId == playerId)
                      || (ip == null || b.IP == ip)
                      || (serial == null || b.Serial == serial)
                      || (socialClubName == null || b.SCName == socialClubName)
                      || (socialClubId == null || b.SCId == socialClubId))
                      && (preventConnection == null || b.PreventConnection == preventConnection);
    }
}
