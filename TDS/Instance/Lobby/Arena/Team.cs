using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
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
            foreach (var list in AlivePlayers)
                if (list.Count >= minalive)
                    ++amount;
            return amount;
        }

        private Teams GetTeamStillInRound(int minalive = 1)
        {
            for (int i = 0; i < AlivePlayers.Length; ++i)
            {
                if (AlivePlayers[i].Count >= minalive)
                    return Teams[i+1];
            }
            return null;
        }

        private Teams GetTeamWithHighestHP()
        {
            int highesthealth = 0;
            Teams teamwithhighesthealth = null;

            foreach (Teams team in Teams)
            {
                if (team.IsSpectatorTeam)
                    continue;
                int teamhealth = 0;
                foreach (var player in AlivePlayers[team.Index])
                {
                    teamhealth += player.Client.Health + player.Client.Armor;
                }
                if (teamhealth > highesthealth)
                {
                    teamwithhighesthealth = team;
                    highesthealth = teamhealth;
                }
            }

            return teamwithhighesthealth;
        }
    }
}
