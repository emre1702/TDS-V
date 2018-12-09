using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Default;
using TDS.Entity;
using TDS.Instance.Player;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        protected readonly Teams[] teams;
        protected readonly List<Character>[] teamPlayers;

        protected void SetPlayerTeam(Character character, Teams team)
        {
            teamPlayers[team.Index].Add(character);
            character.Player.SetSkin(team.SkinHash);
            if (character.Team.Id != team.Id)
                NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.ClientPlayerTeamChange, team.Index);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        private Teams GetTeamWithFewestPlayer()
        {
            int teamindexwithfewest = 0;
            int fewestamount = -1;
            for (int i = 0; i < teamPlayers.Length; ++i) {
                List<Character> entry = teamPlayers[i];
                if (entry.Count > fewestamount
                    || entry.Count == fewestamount && Utils.Rnd.Next(2) == 1)
                {
                    fewestamount = entry.Count;
                    teamindexwithfewest = i;
                }
            }
            return teams[teamindexwithfewest];
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in teamPlayers)
            {
                entry.Clear();
            }
        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (Character character in players)
            {
                if (character.Team.IsSpectatorTeam)
                    teamPlayers[0].Add(character);  // because he is already in that team
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
            }
        }
    }
}
