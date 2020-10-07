using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class ArenaTeamsHandler : RoundFightLobbyTeamsHandler, IArenaTeamsHandler
    {
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public ArenaTeamsHandler(IArena lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, events, langHelper)
        {
            events.TeamPreparation += Events_TeamPreparation;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            Events.TeamPreparation -= Events_TeamPreparation;
        }

        public async Task BalanceCurrentTeams()
        {
            var playersToPutIntoOtherTeam = new List<ITDSPlayer>();

            await Do(teams =>
            {
                var teamWithFewestPlayers = teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
                var teamWithMostPlayers = teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();

                while (teamWithFewestPlayers is { } && teamWithMostPlayers is { }
                    && teamWithMostPlayers.Players.Count - teamWithFewestPlayers.Players.Count > 1)
                {
                    var playerToPutIntoOtherTeam = teamWithMostPlayers.Players.Last();
                    SetPlayerTeam(playerToPutIntoOtherTeam, teamWithFewestPlayers);

                    teamWithFewestPlayers = teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
                    teamWithMostPlayers = teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();
                }
            }).ConfigureAwait(false);

            foreach (var player in playersToPutIntoOtherTeam)
                Lobby.Notifications.Send(lang => string.Format(lang.BALANCE_TEAM_INFO, player.DisplayName));
        }

        private void Events_TeamPreparation()
        {
            if (Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound)
                MixTeams();
        }
    }
}
