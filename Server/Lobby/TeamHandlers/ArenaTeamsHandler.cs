using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class ArenaTeamsHandler : RoundFightLobbyTeamsHandler
    {
        public ArenaTeamsHandler(LobbyDb entity, IRoundFightLobbyEventsHandler events) : base(entity, events)
        {
            events.TeamPreparation += Events_TeamPreparation;
        }

        public void BalanceCurrentTeams()
        {
            var teamWithFewestPlayers = Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
            var teamWithMostPlayers = Teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();

            while (teamWithFewestPlayers is { } && teamWithMostPlayers is { }
                && teamWithMostPlayers.Players.Count - teamWithFewestPlayers.Players.Count > 1)
            {
                var playerToPutIntoOtherTeam = teamWithMostPlayers.Players.Last();
                SetPlayerTeam(playerToPutIntoOtherTeam, teamWithFewestPlayers);
                SendNotification(lang => string.Format(lang.BALANCE_TEAM_INFO, playerToPutIntoOtherTeam.DisplayName));

                teamWithFewestPlayers = Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
                teamWithMostPlayers = Teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();
            }
        }

        private void Events_TeamPreparation()
        {
            if (Entity.LobbyRoundSettings.MixTeamsAfterRound)
                MixTeams();
        }
    }
}
