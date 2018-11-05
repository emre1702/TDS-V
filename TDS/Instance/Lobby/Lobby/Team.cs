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
        private Dictionary<uint, Teams> teamsByID = new Dictionary<uint, Teams>();
        protected Dictionary<Teams, List<Character>> teamPlayers = new Dictionary<Teams, List<Character>>();

        private Teams spectatorTeam;

        public void AddTeam(Teams team)
        {
            teamsByID[team.Id] = team;
            teamPlayers[team] = new List<Character>();
            if (team.IsSpectatorTeam)
                spectatorTeam = team;
        }

        protected void SetPlayerTeam(Character character, Teams team)
        {
            teamPlayers[team].Add(character);
            character.Player.SetSkin(team.SkinHash);
            if (character.Team != team)
                NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvents.ClientPlayerTeamChange, team.Id);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        private Teams GetTeamWithFewestPlayer()
        {
            Teams teamwithfewest = null;
            int fewestamount = -1;
            foreach (var entry in teamPlayers)
            {
                if (entry.Value.Count > fewestamount 
                    || entry.Value.Count == fewestamount && Utils.Rnd.Next(2) == 1)
                {
                    fewestamount = entry.Value.Count;
                    teamwithfewest = entry.Key;
                }
            }
            return teamwithfewest;
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in teamPlayers)
            {
                entry.Value.Clear();
            }
        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (Character character in this.players)
            {
                if (character.Team.IsSpectatorTeam)
                    teamPlayers[spectatorTeam].Add(character);
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
            }
        }
    }
}
