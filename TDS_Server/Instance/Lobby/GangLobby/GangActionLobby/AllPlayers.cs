using GTANetworkAPI;
using System;
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
    }
}
