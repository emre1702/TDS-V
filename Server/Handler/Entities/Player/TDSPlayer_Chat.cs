using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {

        public ITDSPlayer? InPrivateChatWith { get; set; }
        public ITDSPlayer? SentPrivateChatRequestTo { get; set; }

        public void ClosePrivateChat(bool disconnected)
        {
            if (InPrivateChatWith is null && SentPrivateChatRequestTo == null)
                return;

            if (InPrivateChatWith is { })
            {
                if (disconnected)
                {
                    InPrivateChatWith.ModPlayer?.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    ModPlayer?.SendNotification(Language.PRIVATE_CHAT_CLOSED_YOU);
                    InPrivateChatWith.ModPlayer?.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                InPrivateChatWith.InPrivateChatWith = null;
                InPrivateChatWith = null;
            }
            else if (SentPrivateChatRequestTo is { })
            {
                if (!disconnected)
                {
                    ModPlayer?.SendNotification(Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                SentPrivateChatRequestTo.ModPlayer?.SendNotification(
                    SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(DisplayName)
                );
                SentPrivateChatRequestTo = null;
            }
        }

        public void SendMessage(string msg)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (ModPlayer is { })
                NAPI.Chat.SendChatMessageToPlayer(ModPlayer, msg);
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (ModPlayer is { })
                NAPI.Notification.SendNotificationToPlayer(ModPlayer, msg, flashing);
        }
    }
}
