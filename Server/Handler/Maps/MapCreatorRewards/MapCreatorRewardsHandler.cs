using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.Handler.Maps.MapCreatorRewards;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Maps
{
    public class MapCreatorRewardsHandler
    {
        private readonly Dictionary<IRoundFightLobby, Func<ValueTask>> _lobbyRoundEndStatsEventHandlers = new();

        private readonly ISettingsHandler _settingsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly MapCreatorRewardsGiveOnlineHandler _giveOnlineHandler;
        private readonly MapCreatorRewardsGiveOfflineHandler _giveOfflineHandler;

        public MapCreatorRewardsHandler(EventsHandler eventsHandler, ITDSPlayerHandler tdsPlayerHandler, DatabasePlayerHelper databasePlayerHelper,
            ISettingsHandler settingsHandler, MapCreatorRewardsOfflineNotificationHandler offlineNotificationHandler)
        {
            _settingsHandler = settingsHandler;
            _giveOnlineHandler = new MapCreatorRewardsGiveOnlineHandler();
            _giveOfflineHandler = new MapCreatorRewardsGiveOfflineHandler(databasePlayerHelper, offlineNotificationHandler);
            _tdsPlayerHandler = tdsPlayerHandler;

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
                if (!ShouldGiveReward(lobby))
                    return;

                var reward = GetMoneyReward(lobby);
                if (reward == 0)
                    return;

                var creatorId = lobby.CurrentMap.Info.CreatorId!.Value;
                var onlinePlayer = _tdsPlayerHandler.GetPlayer(creatorId);
                if (onlinePlayer is { })
                {
                    _giveOnlineHandler.GiveReward(onlinePlayer, lobby, reward);
                    return;
                }

                await _giveOfflineHandler.GiveReward(creatorId, lobby, reward);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private int GetMoneyReward(IRoundFightLobby lobby)
            => lobby.MapHandler.MapRetrieveType switch
            {
                MapRetrieveType.Random => _settingsHandler.ServerSettings.MapCreatorRewardRandomlySelected,
                MapRetrieveType.Voted => _settingsHandler.ServerSettings.MapCreatorRewardVoted,
                MapRetrieveType.Bought => _settingsHandler.ServerSettings.MapCreatorRewardBought,
                _ => 0
            };

        private bool ShouldGiveReward(IRoundFightLobby lobby)
            => lobby.CurrentMap.Info is { CreatorId: not null and > 0, IsNewMap: false } &&
                lobby.CurrentMap.BrowserSyncedData.CreatorName != "?";
    }
}