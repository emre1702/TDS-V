using System;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Public Properties

        public ITDSPlayer? InPrivateChatWith { get; set; }
        public ITDSPlayer? SentPrivateChatRequestTo { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void ClosePrivateChat(bool disconnected)
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

        public void SendMessage(string msg)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (ModPlayer is { })
                ModPlayer.SendMessage(msg);
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            if (IsConsole)
                Console.WriteLine(msg);
            else if (ModPlayer is { })
                ModPlayer.SendNotification(msg, flashing);
        }

        #endregion Public Methods
    }
}
