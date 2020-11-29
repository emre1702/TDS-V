using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Commands.User
{
    public class UserPrivateChatCommands
    {
        private readonly ChatHandler _chatHandler;

        public UserPrivateChatCommands(ChatHandler chatHandler)
            => _chatHandler = chatHandler;

        [TDSCommand(UserCommand.ClosePrivateChat)]
        public void ClosePrivateChat(ITDSPlayer player)
        {
            if (player.InPrivateChatWith is null && player.SentPrivateChatRequestTo is null)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT));
                return;
            }
            player.Chat.ClosePrivateChat(false);
        }

        [TDSCommand(UserCommand.OpenPrivateChat)]
        public void OpenPrivateChat(ITDSPlayer player, ITDSPlayer target)
        {
            if (CheckIsBlocked(player, target))
                return;
            if (CheckIsAlreadyInPrivateChat(player))
                return;
            if (player.SentPrivateChatRequestTo == target)
                return;
            WithdrawOldPrivateChatRequest(player);
            if (CheckIsTargetAlreadyInPrivatChat(player, target))
                return;

            if (target.SentPrivateChatRequestTo != player)
                SendPrivateChatRequest(player, target);
            else
                AcceptPrivateChatRequest(player, target);
        }

        [TDSCommand(UserCommand.PrivateChat)]
        public void PrivateChat(ITDSPlayer player, [TDSRemainingTextAttribute] string message)
        {
            if (player.InPrivateChatWith is null)
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.NOT_IN_PRIVATE_CHAT));
                return;
            }
            string colorStr = "!$155|35|133$";
            NAPI.Task.RunSafe(() => player.InPrivateChatWith.SendChatMessage($"{colorStr}[{player.DisplayName}: {message}]"));
        }

        [TDSCommand(UserCommand.PrivateMessage)]
        public void PrivateMessage(ITDSPlayer player, ITDSPlayer target, [TDSRemainingTextAttribute] string message)
        {
            if (player == target)
                return;
            if (target.Relations.HasRelationTo(player, PlayerRelation.Block))
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, target.DisplayName)));
                return;
            }
            _chatHandler.SendPrivateMessage(player, target, message);
            NAPI.Task.RunSafe(() => player.SendChatMessage(player.Language.PRIVATE_MESSAGE_SENT));
        }

        private void AcceptPrivateChatRequest(ITDSPlayer player, ITDSPlayer requestFrom)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.PRIVATE_CHAT_OPENED_WITH, requestFrom.DisplayName));
                requestFrom.SendNotification(string.Format(requestFrom.Language.PRIVATE_CHAT_OPENED_WITH, player.DisplayName));
            });
            requestFrom.SentPrivateChatRequestTo = null;
            player.InPrivateChatWith = requestFrom;
            requestFrom.InPrivateChatWith = player;
        }

        private void SendPrivateChatRequest(ITDSPlayer player, ITDSPlayer target)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.PRIVATE_CHAT_REQUEST_SENT_TO, target.DisplayName));
                target.SendNotification(string.Format(target.Language.PRIVATE_CHAT_REQUEST_RECEIVED_FROM, player.DisplayName));
            });
            player.SentPrivateChatRequestTo = target;
        }

        private bool CheckIsTargetAlreadyInPrivatChat(ITDSPlayer player, ITDSPlayer target)
        {
            if (target.InPrivateChatWith != null)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.TARGET_ALREADY_IN_PRIVATE_CHAT));
                return true;
            }
            return false;
        }

        private void WithdrawOldPrivateChatRequest(ITDSPlayer player)
        {
            if (player.SentPrivateChatRequestTo is null)
                return;

            var oldTarget = player.SentPrivateChatRequestTo;
            NAPI.Task.RunSafe(() => oldTarget.SendNotification(string.Format(oldTarget.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER, player.DisplayName)));
            player.SentPrivateChatRequestTo = null;
        }

        private bool CheckIsAlreadyInPrivateChat(ITDSPlayer player)
        {
            if (player.InPrivateChatWith != null)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.ALREADY_IN_PRIVATE_CHAT_WITH, player.InPrivateChatWith.DisplayName)));
                return true;
            }
            return false;
        }

        private bool CheckIsBlocked(ITDSPlayer player, ITDSPlayer heWhoMaybeBlockedMe)
        {
            if (heWhoMaybeBlockedMe.Relations.HasRelationTo(player, PlayerRelation.Block))
            {
                NAPI.Task.RunSafe(() => player.SendChatMessage(string.Format(player.Language.YOU_GOT_BLOCKED_BY, heWhoMaybeBlockedMe.DisplayName)));
                return true;
            }
            return false;
        }
    }
}
