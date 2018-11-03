using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        private List<Character> players = new List<Character>();

        protected void SendAllPlayerEvent(string eventname, uint? teamindex, params object[] args)
        {
            if (!teamindex.HasValue)
            {
                this.FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Player, eventname, args); });
            }
            else
                this.FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Player, eventname, args); }, teamindex.Value);
        }

        protected void FuncIterateAllPlayers(Action<Character, Teams> func, uint? teamID = null)
        {
            if (!teamID.HasValue)
            {
                foreach (var entry in this.TeamPlayers)
                {
                    for (int j = entry.Value.Count - 1; j >= 0; --j)
                    {
                        func(entry.Value[j], entry.Key);
                    }
                }
            }
            else
            {
                for (int j = this.TeamPlayers[this.teamsByID[teamID.Value]].Count - 1; j >= 0; --j)
                {
                    Teams team = this.teamsByID[teamID.Value];
                    func(this.TeamPlayers[team][j], team);
                }
            }
        }

        private void SendAllPlayerLangMessage(Func<ELanguage, string> langgetter, uint? teamindex = null)
        {
            Dictionary<ELanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            this.FuncIterateAllPlayers((character, teamID) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Player, texts[(ELanguage)character.Entity.Playersettings.Language]);
            }, teamindex);
        }

        public void SendAllPlayerChatMessage(string msg, uint? teamindex = null)
        {
            this.FuncIterateAllPlayers((character, teamID) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Player, msg);
            }, teamindex);
        }
    }
}
