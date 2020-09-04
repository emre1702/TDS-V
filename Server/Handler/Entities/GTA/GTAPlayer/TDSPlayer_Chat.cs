using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override ITDSPlayer? InPrivateChatWith { get; set; }
        public override ITDSPlayer? SentPrivateChatRequestTo { get; set; }

        public override void ClosePrivateChat(bool disconnected)
        {
            if (InPrivateChatWith is null && SentPrivateChatRequestTo == null)
                return;

            if (InPrivateChatWith is { })
            {
                if (disconnected)
                {
                    InPrivateChatWith.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    SendNotification(Language.PRIVATE_CHAT_CLOSED_YOU);
                    InPrivateChatWith.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                InPrivateChatWith.InPrivateChatWith = null;
                InPrivateChatWith = null;
            }
            else if (SentPrivateChatRequestTo is { })
            {
                if (!disconnected)
                {
                    SendNotification(Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                SentPrivateChatRequestTo.SendNotification(
                    string.Format(SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER, DisplayName)
                );
                SentPrivateChatRequestTo = null;
            }
        }

        public override void SendChatMessage(string msg)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else
                base.SendChatMessage(msg);
        }

        public override void SendNotification(string msg, bool flashing = false)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else
                base.SendNotification(msg, flashing);
        }
    }
}
