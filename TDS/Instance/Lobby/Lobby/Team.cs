using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Default;
using TDS.Entity;
using TDS.Instance.Player;
using TDS.Manager.Utility;
using TDS_Common.Default;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        protected readonly Teams[] Teams;
        protected readonly List<TDSPlayer>[] TeamPlayers;

        protected void SetPlayerTeam(TDSPlayer character, Teams team)
        {
            TeamPlayers[team.Index].Add(character);
            character.Client.SetSkin(team.SkinHash);
            if (character.Team.Id != team.Id)
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerTeamChange, team.Index);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Teams GetTeamWithFewestPlayer()
        {
            int teamindexwithfewest = 0;
            int fewestamount = -1;
            for (int i = 0; i < TeamPlayers.Length; ++i) {
                List<TDSPlayer> entry = TeamPlayers[i];
                if (entry.Count > fewestamount
                    || entry.Count == fewestamount && Utils.Rnd.Next(2) == 1)
                {
                    fewestamount = entry.Count;
                    teamindexwithfewest = i;
                }
            }
            return Teams[teamindexwithfewest];
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in TeamPlayers)
            {
                entry.Clear();
            }
        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (TDSPlayer character in players)
            {
                if (character.Team.IsSpectatorTeam)
                    TeamPlayers[0].Add(character);  // because he is already in that team
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
            }
        }
    }
}
