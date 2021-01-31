using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.PlayersSystem
{
    public class Chat : IPlayerChat
    {
#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void ClosePrivateChat(bool becauseOfDisconnectOfTarget)
        {
            CloseInPrivateChat(becauseOfDisconnectOfTarget);
            CloseSentPrivateChatRequest(becauseOfDisconnectOfTarget);
        }

        public void SendChatMessage(string msg)
        {
            if (_player.IsConsole)
                Console.WriteLine("[CHAT] " + msg);
            else
                NAPI.Task.RunSafe(() =>
                    ((Player)_player).SendChatMessage(msg));
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            if (_player.IsConsole)
                Console.WriteLine("[NOTIFICATION] " + msg);
            else
                NAPI.Task.RunSafe(() =>
                    ((Player)_player).SendNotification(msg, flashing));
        }

        public void SendAlert(string msg)
        {
            if (_player.IsConsole)
                Console.WriteLine("[ALERT] " + msg);
            else
                NAPI.Task.RunSafe(() =>
                    _player.TriggerEvent(ToClientEvent.SendAlert, msg));
        }

        private void CloseInPrivateChat(bool becauseOfDisconnectOfTarget)
        {
            if (_player.InPrivateChatWith is null)
                return;

            NAPI.Task.RunSafe(() =>
            {
                if (becauseOfDisconnectOfTarget)
                {
                    _player.InPrivateChatWith.SendNotification(_player.InPrivateChatWith.Language.PRIVATE_CHAT_DISCONNECTED);
                }
                else
                {
                    SendNotification(_player.Language.PRIVATE_CHAT_CLOSED_YOU);
                    _player.InPrivateChatWith.SendNotification(_player.InPrivateChatWith.Language.PRIVATE_CHAT_CLOSED_PARTNER);
                }
                _player.InPrivateChatWith.InPrivateChatWith = null;
                _player.InPrivateChatWith = null;
            });
        }

        private void CloseSentPrivateChatRequest(bool becauseOfDisconnectOfTarget)
        {
            if (_player.SentPrivateChatRequestTo is null)
                return;

            NAPI.Task.RunSafe(() =>
            {
                if (!becauseOfDisconnectOfTarget)
                {
                    SendNotification(_player.Language.PRIVATE_CHAT_REQUEST_CLOSED_YOU);
                }
                _player.SentPrivateChatRequestTo.SendNotification(
                    string.Format(_player.SentPrivateChatRequestTo.Language.PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER, _player.DisplayName)
                );
                _player.SentPrivateChatRequestTo = null;
            });
        }
    }
}