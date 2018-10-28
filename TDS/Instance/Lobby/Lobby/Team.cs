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
        private Dictionary<Teams, List<Character>> TeamPlayers = new Dictionary<Teams, List<Character>>();

        public static Teams SpectatorTeam;

        public void AddTeam(Teams team)
        {
            this.teamsByID[team.Id] = team;
            this.TeamPlayers[team] = new List<Character>();
        }

        private void SetPlayerTeam(Character character, Teams team)
        {
            this.TeamPlayers[team].Add(character);
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
            foreach (var entry in this.TeamPlayers)
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
            foreach (var entry in this.TeamPlayers)
            {
                entry.Value.Clear();
            }
        }

        private void MixTeams()
        {
            this.ClearTeamPlayersLists();
            foreach (Character character in this.players)
            {
                if (character.Team == SpectatorTeam)
                    this.TeamPlayers[SpectatorTeam].Add(character);
                else
                    this.SetPlayerTeam(character, this.GetTeamWithFewestPlayer());
            }
        }
    }
}
