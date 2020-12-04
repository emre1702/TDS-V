using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.PlayerHandlers;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Maps
{
    public class MapCreatorRewardsHandler
    {
        private readonly Dictionary<IRoundFightLobby, Func<ValueTask>> _lobbyRoundEndStatsEventHandlers = new();

        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly ISettingsHandler _settingsHandler;
        private readonly OfflineMessagesHandler _offlineMessagesHandler;
        private readonly LangHelper _langHelper;

        public MapCreatorRewardsHandler(EventsHandler eventsHandler, ITDSPlayerHandler tdsPlayerHandler, DatabasePlayerHelper databasePlayerHelper, 
            ISettingsHandler settingsHandler, OfflineMessagesHandler offlineMessagesHandler, LangHelper langHelper)
        {
            _tdsPlayerHandler = tdsPlayerHandler;
            _databasePlayerHelper = databasePlayerHelper;
            _settingsHandler = settingsHandler;
            _offlineMessagesHandler = offlineMessagesHandler;
            _langHelper = langHelper;

            eventsHandler.LobbyCreated += AddLobbyEvents;
            eventsHandler.LobbyRemoved += RemoveLobbyEvents;
        }

        private void AddLobbyEvents(IBaseLobby baseLobby)
        {
            if (baseLobby is not IRoundFightLobby { IsOfficial: true } lobby) 
                return;

            async ValueTask eventHandler() => await OnLobbyRoundEndStats(lobby);
            _lobbyRoundEndStatsEventHandlers[lobby] = eventHandler;
            lobby.Events.RoundEndStats += eventHandler;
        }

        private void RemoveLobbyEvents(IBaseLobby baseLobby)
        {
            if (baseLobby is not IRoundFightLobby { IsOfficial: true } lobby)
                return;
            if (!_lobbyRoundEndStatsEventHandlers.TryGetValue(lobby, out var eventHandler))
                return;
            if (lobby.Events.RoundEndStats is { })
                lobby.Events.RoundEndStats -= eventHandler;
            _lobbyRoundEndStatsEventHandlers.Remove(lobby);
        }

        private async ValueTask OnLobbyRoundEndStats(IRoundFightLobby lobby)
        {
            try
            {
                if (lobby.CurrentMap is null || lobby.CurrentMap.Info.IsNewMap)
                    return;
                if (lobby.CurrentMap.Info.CreatorId is not { } creatorId)
                    return;
                if (creatorId <= 0)
                    return;

                var reward = GetMoneyReward(lobby);
                if (reward == 0)
                    return;

                var onlinePlayer = _tdsPlayerHandler.GetPlayer(creatorId);
                if (onlinePlayer is { })
                {
                    GiveRewardToOnlinePlayer(onlinePlayer, lobby, reward);
                    return;
                }

                await GiveRewardToOfflinePlayer(creatorId, lobby, reward);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private void GiveRewardToOnlinePlayer(ITDSPlayer onlinePlayer, IRoundFightLobby lobby, int reward)
        {
            onlinePlayer.Money += reward;

            lobby.Notifications.Send(lang => string.Format(lang.MAP_CREATOR_REWARD_INFO, lobby.CurrentMap.BrowserSyncedData.CreatorName, reward));

            if (onlinePlayer.Lobby != lobby)
                onlinePlayer.SendNotification(string.Format(onlinePlayer.Language.YOU_GOT_MAP_CREATOR_REWARD, lobby.CurrentMap.BrowserSyncedData.Name, reward));
        }

        private async Task GiveRewardToOfflinePlayer(int playerId, IRoundFightLobby lobby, int reward)
        {
            await UpdateOfflinePlayerStats(playerId, reward);

            lobby.Notifications.Send(lang => string.Format(lang.MAP_CREATOR_REWARD_INFO, lobby.CurrentMap.BrowserSyncedData.CreatorName, reward));
            _offlineMessagesHandler.Add(playerId, await _databasePlayerHelper.GetDiscordUserId(playerId), null,
                string.Format(_langHelper.GetLang(Language.English).YOU_GOT_MAP_CREATOR_REWARD, lobby.CurrentMap.BrowserSyncedData.Name, reward));

            await UpdateOfflinePlayerTotalStats(playerId, reward);
        }

        private async Task UpdateOfflinePlayerStats(int playerId, int reward)
        {
            if (!await DoPlayerStatsExist(playerId))
                return;

            var playerStatsMoney = await GetCurrentPlayerStatsMoney(playerId) + reward;
            var playerStats = new PlayerStats
            {
                PlayerId = playerId,
                Money = playerStatsMoney
            };
            await UpdatePlayerStats(playerStats);
        }

        private async Task UpdateOfflinePlayerTotalStats(int playerId, int reward)
        {
            if (!await DoPlayerTotalStatsExist(playerId))
                return;

            var playerTotalStatsMoney = await GetCurrentPlayerTotalStatsMoney(playerId) + reward;
            var playerTotalStats = new PlayerTotalStats
            {
                PlayerId = playerId,
                Money = playerTotalStatsMoney
            };
            await UpdatePlayerTotalStats(playerTotalStats);
        }

        private Task UpdatePlayerStats(PlayerStats playerStats)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.PlayerStats.Attach(playerStats);
                dbContext.Entry(playerStats).Property(s => s.Money).IsModified = true;

                await dbContext.SaveChangesAsync();
            }, dbContext => dbContext.Entry(playerStats).State = EntityState.Detached);

        private Task<bool> DoPlayerStatsExist(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerStats.AsNoTracking().AnyAsync(s => s.PlayerId == playerId));

        private Task<int> GetCurrentPlayerStatsMoney(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerStats.AsNoTracking().Where(s => s.PlayerId == playerId).Select(s => s.Money).FirstOrDefaultAsync());

        private Task UpdatePlayerTotalStats(PlayerTotalStats playerTotalStats)
           => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
           {
               dbContext.PlayerTotalStats.Attach(playerTotalStats);
               dbContext.Entry(playerTotalStats).Property(s => s.Money).IsModified = true;

               await dbContext.SaveChangesAsync();
           }, dbContext => dbContext.Entry(playerTotalStats).State = EntityState.Detached);

        private Task<bool> DoPlayerTotalStatsExist(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerTotalStats.AsNoTracking().AnyAsync(s => s.PlayerId == playerId));

        private Task<long> GetCurrentPlayerTotalStatsMoney(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerTotalStats.AsNoTracking().Where(s => s.PlayerId == playerId).Select(s => s.Money).FirstOrDefaultAsync());

        private int GetMoneyReward(IRoundFightLobby lobby)
            => lobby.MapHandler.MapRetrieveType switch
            {
                MapRetrieveType.Random => _settingsHandler.ServerSettings.MapCreatorRewardRandomlySelected,
                MapRetrieveType.Voted => _settingsHandler.ServerSettings.MapCreatorRewardVoted,
                MapRetrieveType.Bought => _settingsHandler.ServerSettings.MapCreatorRewardBought,
                _ => 0
            };
    }
}
