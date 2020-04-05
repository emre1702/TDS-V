using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Browser
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
                    _modAPI.Event.Tick.Add(new EventMethodData<Action>(OnUpdate));
                else
                    _modAPI.Event.Tick.Remove(new EventMethodData<Action>(OnUpdate));
            }
        }

        private bool _isOpen;

        private readonly BrowserHandler _browserHandler;
        private readonly IModAPI _modAPI;

        public ChatHandler(BrowserHandler browserHandler, IModAPI modAPI, BindsHandler bindsHandler)
        {
            _browserHandler = browserHandler;
            _modAPI = modAPI;

            modAPI.Chat.Show(false);
            bindsHandler.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            bindsHandler.Add(Control.MpTextChatTeam, OpenTeamChatInput);
            bindsHandler.Add(Key.Escape, (_) => CloseChatInput());
        }

        public void OnUpdate()
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

        public void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            _browserHandler.Angular.ToggleChatInput(false);
        }
    }
}
