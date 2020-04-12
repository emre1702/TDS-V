using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class ChatHandler
    {
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                _browserHandler.Angular.ToggleChatOpened(value);
                if (value)
                    _modAPI.Event.Tick.Add(_tickEventMethod);
                else
                    _modAPI.Event.Tick.Remove(_tickEventMethod);
            }
        }

        private bool _isOpen;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly BrowserHandler _browserHandler;
        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly BombHandler _bombHandler;
        private readonly PlayerFightHandler _playerFightHandler;

        public ChatHandler(BrowserHandler browserHandler, IModAPI modAPI, BindsHandler bindsHandler, RemoteEventsSender remoteEventsSender,
            BombHandler bombHandler, PlayerFightHandler playerFightHandler)
        {
            _browserHandler = browserHandler;
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
            _bombHandler = bombHandler;
            _playerFightHandler = playerFightHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(OnUpdate);

            modAPI.Chat.Show(false);
            bindsHandler.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            bindsHandler.Add(Control.MpTextChatTeam, OpenTeamChatInput);
            bindsHandler.Add(Key.Escape, (_) => CloseChatInput());

            modAPI.Event.Add(FromBrowserEvent.CloseChat, _ => CloseChatInput());
            modAPI.Event.Add(FromBrowserEvent.ChatUsed, OnChatUsedMethod);
            modAPI.Event.Add(FromBrowserEvent.CommandUsed, OnCommandUsedMethod);
        }

        public void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            _browserHandler.Angular.ToggleChatInput(false);
        }

        private void OnUpdate(ulong _)
        {
            _modAPI.Control.DisableAllControlActions(InputGroup.LOOK);
            _modAPI.Control.DisableAllControlActions(InputGroup.MOVE);
            _modAPI.Control.DisableAllControlActions(InputGroup.SUB);
        }

        private void OpenLobbyChatInput(Control _)
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput(null);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void OpenGlobalChatInput(Control _)
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput("/globalsay ");
        }

        private void OpenTeamChatInput(Control _)
        {
            if (_browserHandler.InInput)
                return;

            OpenChatInput("/teamsay ");
        }

        private void OpenChatInput(string cmd)
        {
            if (IsOpen)
                return;
            IsOpen = true;

            if (cmd == null)
                _browserHandler.Angular.ToggleChatInput(true);
            else
                _browserHandler.Angular.ToggleChatInput(true, cmd);
        }

        private void OnChatUsedMethod(object[] args)
        {
            CloseChatInput();
            string msg = (string)args[0];
            int chatTypeNumber = (int)(args[1]);
            _remoteEventsSender.Send(ToServerEvent.LobbyChatMessage, msg, chatTypeNumber);
        }

        private void OnCommandUsedMethod(object[] args)
        {
            CloseChatInput();
            string msg = (string)args[0];
            if (msg == "checkshoot")
            {
                if (_bombHandler.BombOnHand || !_playerFightHandler.InFight)
                    _modAPI.Chat.Output("Shooting is blocked. Reason: " + (_playerFightHandler.InFight ? "bomb" : (!_bombHandler.BombOnHand ? "round" : "both")));
                else
                    _modAPI.Chat.Output("Shooting is not blocked.");
                return;
            }

            _remoteEventsSender.Send(ToServerEvent.CommandUsed, msg);
        }
    }
}
