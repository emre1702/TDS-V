using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public readonly List<TDSPlayer> Players = new List<TDSPlayer>();

        protected void SendAllPlayerEvent(string eventname, uint? teamindex, params object[] args)
        {
            if (!teamindex.HasValue)
            {
                FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Client, eventname, args); });
            }
            else
                FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Client, eventname, args); }, teamindex.Value);
        }

        protected void FuncIterateAllPlayers(Action<TDSPlayer, Teams> func, uint? teamIndex = null)
        {
            if (!teamIndex.HasValue)
            {
                for (int i = 0; i < TeamPlayers.Length; ++i)
                {
                    Teams team = Teams[i];
                    for (int j = TeamPlayers[i].Count - 1; j >= 0; --j)
                    {
                        func(TeamPlayers[i][j], team);
                    }
                }
            }
            else
            {
                uint i = teamIndex.Value;
                Teams team = Teams[teamIndex.Value];
                for (int j = TeamPlayers[i].Count - 1; j >= 0; --j)
                {
                    func(TeamPlayers[i][j], team);
                }
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, uint? teamindex = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Client, texts[character.Language]);
            }, teamindex);
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, uint? teamindex = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.Notification.SendNotificationToPlayer(character.Client, texts[character.Language]);
            }, teamindex);
        }

        public void SendAllPlayerChatMessage(string msg, uint? teamindex = null)
        {
            FuncIterateAllPlayers((character, teamID) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Client, msg);
            }, teamindex);
        }
    }
}
