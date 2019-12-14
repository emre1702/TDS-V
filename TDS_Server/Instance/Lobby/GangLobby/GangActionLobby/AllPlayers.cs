using GTANetworkAPI;
using System;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;

namespace TDS_Server.Instance.Lobby
{
    partial class GangActionLobby
    {
        public void SendLangMessageToAttacker(Func<ILanguage, string> langGetter)
        {
            foreach (var player in AttackerGang.PlayersOnline)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, langGetter(player.Language));
            }
        }

        public void SendLangMessageToOwner(Func<ILanguage, string> langGetter)
        {
            foreach (var player in OwnerGang.PlayersOnline)
            {
                NAPI.Chat.SendChatMessageToPlayer(player.Client, langGetter(player.Language));
            }
        }

        public void SendLangNotificationToAttacker(Func<ILanguage, string> langGetter)
        {
            foreach (var player in AttackerGang.PlayersOnline)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, langGetter(player.Language));
            }
        }

        public void SendLangNotificationToOwner(Func<ILanguage, string> langGetter)
        {
            foreach (var player in OwnerGang.PlayersOnline)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, langGetter(player.Language));
            }
        }

        public void FuncIterateAttackerInGangLobby(Action<TDSPlayer> action)
        {
            foreach (var player in AttackerGang.GangLobbyTeam.Players)
            {
                action(player);
            }
        }

        public void FuncIterateOwnerInGangLobby(Action<TDSPlayer> action)
        {
            foreach (var player in OwnerGang.GangLobbyTeam.Players)
            {
                action(player);
            }
        }
    }
}
