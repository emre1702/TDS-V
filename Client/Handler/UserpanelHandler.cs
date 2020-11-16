using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.Userpanel;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class UserpanelHandler : ServiceBase
    {
        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly SettingsHandler _settingsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private bool _open;

        public UserpanelHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, CursorHandler cursorHandler, SettingsHandler settingsHandler,
            RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler, BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _cursorHandler = cursorHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;

            _bindsHandler = bindsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            RAGE.Events.Add(FromBrowserEvent.CloseUserpanel, _ => Close());
            RAGE.Events.Add(ToServerEvent.LoadUserpanelData, OnLoadUserpanelDataBrowserMethod);
        }

        public void Close()
        {
            Toggle(Key.Noname);
        }

        public void Toggle(Key key)
        {
            if (key != Key.Noname)
            {
                if (_browserHandler.InInput)
                    return;
            }
            else
                _open = true;

            _open = !_open;
            _cursorHandler.Visible = _open;
            _browserHandler.Angular.ToggleUserpanel(_open);

            if (!_open)
                _settingsHandler.RevertTempSettings();
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Key.U, Toggle);
            _instructionalButtonHandler.Add("Userpanel", "U", true);
        }

        private void OnLoadUserpanelDataBrowserMethod(object[] args)
        {
            UserpanelLoadDataType type = (UserpanelLoadDataType)Convert.ToInt32(args[0]);
            switch (type)
            {
                case UserpanelLoadDataType.SettingsCommands:
                    _browserHandler.Angular.LoadUserpanelData((int)type, Serializer.ToBrowser(_settingsHandler.CommandsData));
                    break;

                default:
                    _remoteEventsSender.Send(ToServerEvent.LoadUserpanelData, (int)type);
                    break;
            }

            _settingsHandler.RevertTempSettings();
        }
    }
}
