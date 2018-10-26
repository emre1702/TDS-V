using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.Enum;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        private List<Client> players = new List<Client>();
        private List<List<Client>> TeamPlayers = new List<List<Client>>();

        private void SendAllPlayerEvent(string eventname, int? teamindex, params object[] args)
        {
            if (!teamindex.HasValue)
            {
                this.FuncIterateAllPlayers((player, teamID) => { NAPI.ClientEvent.TriggerClientEvent(player, eventname, args); });
            }
            else
                this.FuncIterateAllPlayers((player, teamID) => { NAPI.ClientEvent.TriggerClientEvent(player, eventname, args); }, teamindex.Value);
        }

        private void FuncIterateAllPlayers(Action<Client, int> func, int? teamID = null)
        {
            if (!teamID.HasValue)
            {
                for (int i = 0; i < this.TeamPlayers.Count; i++)
                {
                    for (int j = this.TeamPlayers[i].Count - 1; j >= 0; j--)
                    {
                        Client player = this.TeamPlayers[i][j];
                        func(player, i);
                    }
                }
            }
            else
            {
                for (int j = this.TeamPlayers[teamID.Value].Count - 1; j >= 0; j--)
                {
                    Client player = this.TeamPlayers[teamID.Value][j];
                    func(player, teamID.Value);
                }
            }
        }
    }
}
