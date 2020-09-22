using TDS_Server.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class ArenaTeamsHandler : FightLobbyTeamsHandler
    {
        public ArenaTeamsHandler(LobbyDb entity, BaseLobbyEventsHandler events) : base(entity, events)
        {
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
    }
}
