using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interfaces;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Lobby
    {
        public List<TDSPlayer> Players { get; } = new List<TDSPlayer>();

        public void SendAllPlayerEvent(string eventname, Team? team, params object[] args)
        {
            if (team is null)
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Select(p => p.Client).ToArray(), eventname, args);
            else
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p.Team == team).Select(p => p.Client).ToArray(), eventname, args);
        }

        public void FuncIterateAllPlayers(Action<TDSPlayer, Team?> func)
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

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, Team? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    NAPI.Notification.SendNotificationToPlayer(character.Client, texts[character.Language], flashing);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    NAPI.Notification.SendNotificationToPlayer(character.Client, texts[character.Language], flashing);
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

        public void SendAllPlayerChatMessage(string msg, HashSet<int> blockingPlayerIds, Team? targetTeam = null)
        {
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, msg);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    NAPI.Chat.SendChatMessageToPlayer(character.Client, msg);
                });
            }
        }

        public void PlaySound(string soundName)
        {
            FuncIterateAllPlayers((player, team) =>
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayCustomSound, soundName)
            );
        }
    }
}