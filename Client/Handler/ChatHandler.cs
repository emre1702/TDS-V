using RAGE.Game;
using System.Collections.Generic;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler
{
    public class ChatHandler : ServiceBase
    {
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                _eventsHandler.OnChatInputToggled(value);
                if (value)
                    Tick += OnUpdate;
                else
                    Tick -= OnUpdate;
            }
        }

        private bool _isOpen;

        private readonly BrowserHandler _browserHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly EventsHandler _eventsHandler;
        private readonly BindsHandler _bindsHandler;

        public ChatHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, BindsHandler bindsHandler,
            RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _remoteEventsSender = remoteEventsSender;
            _eventsHandler = eventsHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            Add(FromBrowserEvent.CloseChat, _ => CloseChatInput());
            Add(FromBrowserEvent.ChatUsed, OnChatUsedMethod);
        }

        public void CloseChatInput(bool force = false)
        {
            if (!IsOpen && !force)
                return;
            IsOpen = false;
            _browserHandler.Angular.ToggleChatInput(false);
        }

        private void OnUpdate(List<TickNametagData> _)
        {
            RAGE.Game.Pad.DisableAllControlActions((int)InputGroup.LOOK);
            RAGE.Game.Pad.DisableAllControlActions((int)InputGroup.MOVE);
            RAGE.Game.Pad.DisableAllControlActions((int)InputGroup.SUB);
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

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Control.MpTextChatAll, OpenLobbyChatInput);
            _bindsHandler.Add(Control.MpTextChatTeam, OpenTeamChatInput);
            _bindsHandler.Add(Key.Escape, (_) => CloseChatInput());
        }
    }
}