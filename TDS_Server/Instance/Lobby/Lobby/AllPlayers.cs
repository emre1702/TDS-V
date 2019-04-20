using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SendAllPlayerEvent(string eventname, Team? team, params object[] args)
        {
            if (team is null)
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Select(p => p.Client).ToArray(), eventname, args);
            else
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p.Team == team).Select(p => p.Client).ToArray(), eventname, args);
        }

        protected void FuncIterateAllPlayers(Action<TDSPlayer, Team?> func)
        {
            foreach (var player in Players)
            {
                func(player, player.Team);
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, Team? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            if (targetTeam is null)
                FuncIterateAllPlayers((player, team) =>
                {
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, texts[player.Language]);
                });
            else
                targetTeam.FuncIterate((player, team) =>
                {
                    NAPI.Chat.SendChatMessageToPlayer(player.Client, texts[player.Language]);
                });
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, Team? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            if (targetTeam is null)
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

        public void SendAllPlayerChatMessage(string msg, Team? targetTeam = null)
        {
            if (targetTeam is null)
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
