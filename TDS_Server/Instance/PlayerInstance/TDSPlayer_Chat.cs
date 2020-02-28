using GTANetworkAPI;
using System;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.PlayerInstance
{
    partial class TDSPlayer
    {

        public TDSPlayer? InPrivateChatWith { get; set; }
        public TDSPlayer? SentPrivateChatRequestTo { get; set; }

        public void ClosePrivateChat(bool disconnected)
        {
            if (InPrivateChatWith is null && SentPrivateChatRequestTo == null)
                return;

            if (InPrivateChatWith is { })
            {
                if (disconnected)
                {
                    InPrivateChatWith.Player?.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    Player?.SendNotification(Language.PRIVATE_CHAT_CLOSED_YOU);
                    InPrivateChatWith.Player?.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                InPrivateChatWith.InPrivateChatWith = null;
                InPrivateChatWith = null;
            }
            else if (SentPrivateChatRequestTo is { })
            {
                if (!disconnected)
                {
                    Player?.SendNotification(Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                SentPrivateChatRequestTo.Player?.SendNotification(
                    SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(DisplayName)
                );
                SentPrivateChatRequestTo = null;
            }
        }

        public void SendMessage(string msg)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (Player is { })
                NAPI.Chat.SendChatMessageToPlayer(Player, msg);
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (Player is { })
                NAPI.Notification.SendNotificationToPlayer(Player, msg, flashing);
        }
    }
}
