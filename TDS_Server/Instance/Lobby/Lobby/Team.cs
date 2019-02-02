using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        protected Teams[] Teams { get; set; }
        protected readonly List<TDSPlayer>[] TeamPlayers;
        protected SyncedTeamDataDto[] SyncedTeamDatas { get; set; }

        protected void SetPlayerTeam(TDSPlayer character, Teams team)
        {
            if (character.Team != null)
                TeamPlayers[character.Team.Index].Remove(character);
            TeamPlayers[team.Index].Add(character);
            character.Client.SetSkin((PedHash)team.SkinHash);
            if (character.Team == null || character.Team.Id != team.Id)
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerTeamChange, team.Name);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Teams GetTeamWithFewestPlayer()
        {
            int teamindexwithfewest = 1;
            int fewestamount = int.MaxValue;
            int length = TeamPlayers.Length;
            for (int i = 1; i < length; ++i) {
                List<TDSPlayer> entry = TeamPlayers[i];
                if (entry.Count < fewestamount
                    || entry.Count == fewestamount && Utils.Rnd.Next(length - 1) == 1)
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
            foreach (TDSPlayer character in Players)
            {
                if (character.Team == null)
                    continue;
                if (character.Team.Index == 0)
                    TeamPlayers[0].Add(character);  // because he is already in that team
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
            }
        }

        protected void ClearTeamPlayersAmounts()
        {
            foreach (var team in SyncedTeamDatas)
            {
                team.AmountPlayers.AmountAlive = 0;
                team.AmountPlayers.Amount = 0;
            }
        }
    }
}
