using System.Linq;
using TDS_Server.Handler.Entities.TeamSystem;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        /// <summary>
        /// Returns the amount of teams with enough players alive.
        /// Used to check if round ended.
        /// </summary>
        /// <param name="minalive">Amount of alive players needed in a team to get considered as "still in round".</param>
        /// <returns>Amount teams still in round.</returns>
        private int GetTeamAmountStillInRound(int minalive = 1)
        {
            return Teams.Count(team => team.AlivePlayers != null && team.AlivePlayers.Count >= minalive);
        }

        private int GetTeamAmount(bool onlyCheckPlayerAmount)
        {
            if (!onlyCheckPlayerAmount)
                return Teams.Count - 1;
            else
                return Teams.Skip(1).Count(team => team.Players.Count > 0);
        }

        private Team? GetTeamStillInRound(int minalive = 1)
        {
            foreach (var team in Teams)
            {
                if (team.AlivePlayers is null)
                    continue;
                if (team.AlivePlayers.Count >= minalive)
                    return team;
            }
            return null;
        }

        private Team? GetTeamWithHighestHP()
        {
            int highesthealth = 0;
            Team? teamwithhighesthealth = Teams[1];

            foreach (Team team in Teams)
            {
                if (team.AlivePlayers is null)
                    continue;
                int teamhealth = 0;
                foreach (var player in team.AlivePlayers)
                {
                    teamhealth += player.Health + player.Armor;
                }
                if (teamhealth > highesthealth)
                {
                    teamwithhighesthealth = team;
                    highesthealth = teamhealth;
                }
                else if (teamhealth == highesthealth)
                    teamwithhighesthealth = null;
            }

            return teamwithhighesthealth;
        }
    }
}
