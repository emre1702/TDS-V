using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler.Account
{
    public class BansHandler : DatabaseEntityWrapper
    {
        private readonly EventsHandler _eventsHandler;

        private readonly LobbiesHandler _lobbiesHandler;

        private readonly ISettingsHandler _settingsHandler;

        private List<PlayerBans> _cachedBans = new List<PlayerBans>();

        public BansHandler(TDSDbContext dbContext, ILoggingHandler logger, LobbiesHandler lobbiesHandler, EventsHandler eventsHandler,
            ISettingsHandler settingsHandler)
            : base(dbContext, logger)
        {
            _lobbiesHandler = lobbiesHandler;
            _settingsHandler = settingsHandler;
            _eventsHandler = eventsHandler;

            eventsHandler.Hour += RemoveExpiredBans;
            eventsHandler.Minute += RefreshServerBansCache;

            eventsHandler.IncomingConnection += CheckBanOnIncomingConnection;
        }

        public void AddServerBan(PlayerBans ban)
        {
            RemoveServerBan(ban);
            lock (_cachedBans)
            {
                _cachedBans.Add(ban);
            }
        }

        public void RemoveServerBan(PlayerBans oldBan)
        {
            lock (_cachedBans)
            {
                var ban = _cachedBans.FirstOrDefault(b => b.PlayerId == oldBan.PlayerId && b.LobbyId == oldBan.LobbyId);
                _cachedBans.Remove(oldBan);
            }
        }

        public async Task<PlayerBans?> GetBan(int lobbyId,
                    int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            PlayerBans? ban = (playerId, ip, serial, socialClubName, socialClubId, andConnection) switch
            {
                ({ }, null, null, null, null, _)
                    => await ExecuteForDBAsync(async (dbContext) =>
                        {
                            return await dbContext.PlayerBans.FirstOrDefaultAsync(b => b.PlayerId == playerId && b.LobbyId == lobbyId);
                        }),

                (_, _, _, _, _, true)
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId
                                && (playerId == null || b.PlayerId == playerId)
                                && (ip == null || b.IP == ip)
                                && (serial == null || b.Serial == serial)
                                && (socialClubName == null || b.SCName == socialClubName)
                                && (socialClubId == null || b.SCId == socialClubId)
                                && (preventConnection == null || b.PreventConnection == preventConnection))
                            .FirstOrDefaultAsync();
                    }),

                (_, _, _, _, _, false)
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId && (
                                (playerId == null || b.PlayerId == playerId)
                                || (ip == null || b.IP == ip)
                                || (serial == null || b.Serial == serial)
                                || (socialClubName == null || b.SCName == socialClubName)
                                || (socialClubId == null || b.SCId == socialClubId)
                                || (preventConnection == null || b.PreventConnection == preventConnection)))
                            .FirstOrDefaultAsync();
                    }),
            };

            if (ban is null)
                return null;

            if (ban.EndTimestamp <= DateTime.UtcNow)
            {
                await ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Entry(ban).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                });
                return null;
            }

            return ban;
        }

        public PlayerBans? GetServerBan(int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            int lobbyId = _lobbiesHandler.MainMenu.Id;
            lock (_cachedBans)
            {
                return (playerId, ip, serial, socialClubName, socialClubId, andConnection) switch
                {
                    ({ }, null, null, null, null, _)
                        => _cachedBans.FirstOrDefault(b => b.PlayerId == playerId && b.LobbyId == lobbyId),

                    (_, _, _, _, _, true)
                        => _cachedBans
                                .Where(b => b.LobbyId == lobbyId
                                    && b.EndTimestamp > DateTime.UtcNow
                                    && (playerId is null || b.PlayerId == playerId)
                                    && (ip is null || b.IP == ip)
                                    && (serial is null || b.Serial == serial)
                                    && (socialClubName is null || b.SCName == socialClubName)
                                    && (socialClubId is null || b.SCId == socialClubId)
                                    && (preventConnection is null || b.PreventConnection == preventConnection))
                                .FirstOrDefault(),

                    (_, _, _, _, _, false)
                        => _cachedBans
                                .Where(b => b.LobbyId == lobbyId
                                    && b.EndTimestamp > DateTime.UtcNow
                                    && ((playerId is null || b.PlayerId == playerId)
                                    || (ip is null || b.IP == ip)
                                    || (serial is null || b.Serial == serial)
                                    || (socialClubName is null || b.SCName == socialClubName)
                                    || (socialClubId is null || b.SCId == socialClubId))
                                    && (preventConnection is null || b.PreventConnection == preventConnection))
                                .FirstOrDefault()
                };
            }
        }

        public async void RefreshServerBansCache(int counter)
        {
            if (counter % _settingsHandler.ServerSettings.ReloadServerBansEveryMinutes != 0)
                return;

            int lobbyId = _lobbiesHandler.MainMenu.Id;
            var entries = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerBans.Where(b => b.LobbyId == lobbyId).Include(b => b.Admin).ToListAsync());
            lock (_cachedBans)
            {
                _cachedBans = entries;
            }

            NAPI.Task.Run(() => _eventsHandler.OnLoadedServerBans());
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
            await ExecuteForDBAsync(async (dbContext) =>
            {
                var bans = await dbContext.PlayerBans
                    .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.UtcNow)
                    .ToListAsync();
                dbContext.PlayerBans.RemoveRange(bans);
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
