using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Lobby
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
            int amount = 0;
            foreach (var team in Teams)
            {
                if (team.AlivePlayers == null)
                    continue;
                if (team.AlivePlayers.Count >= minalive)
                    ++amount;
            }
            return amount;
        }

        private Team? GetTeamStillInRound(int minalive = 1)
        {
            foreach (var team in Teams)
            {
                if (team.AlivePlayers == null)
                    continue;
                if (team.AlivePlayers.Count > 0)
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
                if (team.AlivePlayers == null)
                    continue;
                int teamhealth = 0;
                foreach (var player in team.AlivePlayers)
                {
                    teamhealth += player.Client.Health + player.Client.Armor;
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