using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class RoundFightLobbyPlayers : FightLobbyPlayers
    {
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;

        protected bool SavePlayerLobbyStats { get; private set; }

        public RoundFightLobbyPlayers(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
            events.InitNewMap += Events_InitNewMap;
            events.PlayersPreparation += Events_PlayersPreparation;
            events.InRound += Events_InRound;
            events.RoundEnd += Events_RoundEnd;
            events.RoundEndStats += Events_RoundEndStats;
            events.RoundEndRanking += Events_RoundEndRanking;
        }

        private void Events_InRound()
        {
            //Todo: Implement this
            //StartRoundForAllPlayer();
        }

        protected virtual void Events_PlayersPreparation()
        {
        }

        private void Events_InitNewMap(MapDto mapDto)
        {
            SavePlayerLobbyStats = !mapDto.Info.IsNewMap;
        }

        protected virtual async ValueTask Events_RoundEnd()
        {
            var mapId = Lobby.CurrentMap?.BrowserSyncedData.Id ?? 0;
            await DoInMain(player =>
            {
                var noTeamOrSpectator = player.Team is null || player.Team.IsSpectator;
                var roundEndReasonText = Lobby.CurrentRoundEndReason.MessageProvider(player.Language);

                player.TriggerEvent(ToClientEvent.RoundEnd, noTeamOrSpectator, roundEndReasonText, mapId);
                player.Lifes = 0;
            });
        }

        private async ValueTask Events_RoundEndStats()
        {
            //Todo: RewardAllPlayer
            //Todo: SaveAllPlayerRoundStats
        }

        private void Events_RoundEndRanking()
        {
            var ranking = GetOrderedRoundRanking();
        }
    }
}
