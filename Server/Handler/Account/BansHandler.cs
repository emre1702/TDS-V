using AltV.Net;
using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler.Account
{
    public class BansHandler : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly EventsHandler _eventsHandler;

        private readonly LobbiesHandler _lobbiesHandler;

        private readonly ISettingsHandler _settingsHandler;

        private List<PlayerBans> _cachedBans = new List<PlayerBans>();

        #endregion Private Fields

        #region Public Constructors

        public BansHandler(TDSDbContext dbContext, ILoggingHandler logger, LobbiesHandler lobbiesHandler, EventsHandler eventsHandler,
            ISettingsHandler settingsHandler)
            : base(dbContext, logger)
        {
            _lobbiesHandler = lobbiesHandler;
            _settingsHandler = settingsHandler;
            _eventsHandler = eventsHandler;

            eventsHandler.Hour += RemoveExpiredBans;
            eventsHandler.Minute += RefreshServerBansCache;

            Alt.OnPlayerConnect += (player, reason) => CheckBanOnIncomingConnection((ITDSPlayer)player, reason);
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddServerBan(PlayerBans ban)
        {
            _cachedBans.Add(ban);
        }

        public async Task<PlayerBans?> GetBan(int lobbyId,
                    int? playerId = null, string? ip = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            PlayerBans? ban = (playerId, ip, socialClubId, andConnection) switch
            {
                ({ }, null, null, _)
                    => await ExecuteForDBAsync(async (dbContext) =>
                        {
                            return await dbContext.PlayerBans.FirstOrDefaultAsync(b => b.PlayerId == playerId && b.LobbyId == lobbyId);
                        }),

                (_, _, _, true)
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId
                                && (playerId == null || b.PlayerId == playerId)
                                && (ip == null || b.IP == ip)
                                && (socialClubId == null || b.SCId == socialClubId)
                                && (preventConnection == null || b.PreventConnection == preventConnection))
                            .FirstOrDefaultAsync();
                    }),

                (_, _, _, false)
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId && (
                                (playerId == null || b.PlayerId == playerId)
                                || (ip == null || b.IP == ip)
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

        public PlayerBans? GetServerBan(int? playerId = null, string? ip = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            int lobbyId = _lobbiesHandler.MainMenu.Id;
            PlayerBans? ban = (playerId, ip, socialClubId, andConnection) switch
            {
                ({ }, null, null, _)
                    => _cachedBans.FirstOrDefault(b => b.PlayerId == playerId && b.LobbyId == lobbyId),

                (_, _, _, true)
                    => _cachedBans
                            .Where(b => b.LobbyId == lobbyId
                                && b.EndTimestamp > DateTime.UtcNow
                                && (playerId is null || b.PlayerId == playerId)
                                && (ip is null || b.IP == ip)
                                && (socialClubId is null || b.SCId == socialClubId)
                                && (preventConnection is null || b.PreventConnection == preventConnection))
                            .FirstOrDefault(),

                (_, _, _, false)
                    => _cachedBans
                            .Where(b => b.LobbyId == lobbyId
                                && b.EndTimestamp > DateTime.UtcNow
                                && ((playerId is null || b.PlayerId == playerId)
                                || (ip is null || b.IP == ip)
                                || (socialClubId is null || b.SCId == socialClubId))
                                && (preventConnection is null || b.PreventConnection == preventConnection))
                            .FirstOrDefault()
            };

            return ban;
        }

        public async void RefreshServerBansCache(int counter)
        {
            if (counter % _settingsHandler.ServerSettings.ReloadServerBansEveryMinutes != 0)
                return;

            int lobbyId = _lobbiesHandler.MainMenu.Id;
            _cachedBans = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerBans.Where(b => b.LobbyId == lobbyId).Include(b => b.Admin).ToListAsync());

            await AltAsync.Do(() =>
            {
                _eventsHandler.OnLoadedServerBans();
            });

        }

        public void RemoveServerBanByPlayerId(PlayerBans ban)
        {
            var banToRemove = _cachedBans.FirstOrDefault(b => b.LobbyId == ban.LobbyId && b.PlayerId == ban.PlayerId);
            if (banToRemove is { })
                _cachedBans.Remove(banToRemove);
        }

        #endregion Public Methods

        #region Private Methods

        private void CheckBanOnIncomingConnection(ITDSPlayer player, string reason)
        {
            var ban = GetServerBan(null, player.Ip, player.SocialClubId, true);
            if (ban is { })
                Utils.HandleBan(player, ban);
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

        #endregion Private Methods
    }
}
