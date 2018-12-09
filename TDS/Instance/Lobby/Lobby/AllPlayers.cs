using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Interface;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        private readonly List<Character> players = new List<Character>();

        protected void SendAllPlayerEvent(string eventname, uint? teamindex, params object[] args)
        {
            if (!teamindex.HasValue)
            {
                FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Player, eventname, args); });
            }
            else
                FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Player, eventname, args); }, teamindex.Value);
        }

        protected void FuncIterateAllPlayers(Action<Character, Teams> func, uint? teamIndex = null)
        {
            if (!teamIndex.HasValue)
            {
                for (int i = 0; i < teamPlayers.Length; ++i)
                {
                    Teams team = teams[i];
                    for (int j = teamPlayers[i].Count - 1; j >= 0; --j)
                    {
                        func(teamPlayers[i][j], team);
                    }
                }
            }
            else
            {
                uint i = teamIndex.Value;
                Teams team = teams[teamIndex.Value];
                for (int j = teamPlayers[i].Count - 1; j >= 0; --j)
                {
                    func(teamPlayers[i][j], team);
                }
            }
        }

        protected void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, uint? teamindex = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Player, texts[character.Language]);
            }, teamindex);
        }

        protected void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, uint? teamindex = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.Notification.SendNotificationToPlayer(character.Player, texts[character.Language]);
            }, teamindex);
        }

        public void SendAllPlayerChatMessage(string msg, uint? teamindex = null)
        {
            FuncIterateAllPlayers((character, teamID) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Player, msg);
            }, teamindex);
        }
    }
}
