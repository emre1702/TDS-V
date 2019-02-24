using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public readonly List<TDSPlayer> Players = new List<TDSPlayer>();

        protected void SendAllPlayerEvent(string eventname, Team team, params object[] args)
        {
            if (team == null)
            {
                FuncIterateAllPlayers((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Client, eventname, args); });
            }
            else
                team.FuncIterate((character, teamID) => { NAPI.ClientEvent.TriggerClientEvent(character.Client, eventname, args); });
        }

        protected void FuncIterateAllPlayers(Action<TDSPlayer, Team> func)
        {
            foreach (var team in Teams)
            {
                team.FuncIterate(func);
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, Team targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            targetTeam.FuncIterate((character, team) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(character.Client, texts[character.Language]);
            });
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, Team targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            if (targetTeam == null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    NAPI.Notification.SendNotificationToPlayer(character.Client, texts[character.Language]);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    NAPI.Notification.SendNotificationToPlayer(character.Client, texts[character.Language]);
                });
            }
        }

        public void SendAllPlayerChatMessage(string msg, Team targetTeam = null)
        {
            if (targetTeam == null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, msg);
                });
            } 
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, msg);
                });
            }
        }
    }
}
