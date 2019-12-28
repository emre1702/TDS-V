using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Player
{
    partial class TDSPlayer
    {

        public bool ChatLoaded { get; set; }
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
                    InPrivateChatWith.Client.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    Client.SendNotification(Language.PRIVATE_CHAT_CLOSED_YOU);
                    InPrivateChatWith.Client.SendNotification(InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                InPrivateChatWith.InPrivateChatWith = null;
                InPrivateChatWith = null;
            }
            else if (SentPrivateChatRequestTo is { })
            {
                if (!disconnected)
                {
                    Client.SendNotification(Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                SentPrivateChatRequestTo.Client.SendNotification(
                    SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER.Formatted(DisplayName)
                );
                SentPrivateChatRequestTo = null;
            }
        }
    }
}
