using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class UserpanelHandler : ServiceBase
    {
        #region Private Fields

        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;
        private bool _open;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelHandler(IModAPI modAPI, LoggingHandler loggingHandler, BrowserHandler browserHandler, CursorHandler cursorHandler, SettingsHandler settingsHandler,
            RemoteEventsSender remoteEventsSender, Serializer serializer, EventsHandler eventsHandler, BindsHandler bindsHandler)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _cursorHandler = cursorHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _serializer = serializer;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            modAPI.Event.Add(FromBrowserEvent.CloseUserpanel, _ => Close());
            modAPI.Event.Add(ToServerEvent.LoadUserpanelData, OnLoadUserpanelDataBrowserMethod);
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Key.U, Toggle);
        }

        private void OnLoadUserpanelDataBrowserMethod(object[] args)
        {
            UserpanelLoadDataType type = (UserpanelLoadDataType)Convert.ToInt32(args[0]);
            switch (type)
            {
                case UserpanelLoadDataType.SettingsNormal:
                    _browserHandler.Angular.LoadUserpanelData((int)type, _serializer.ToBrowser(_settingsHandler.PlayerSettings));
                    break;

                case UserpanelLoadDataType.SettingsCommands:
                    _browserHandler.Angular.LoadUserpanelData((int)type, _serializer.ToBrowser(_settingsHandler.CommandsData));
                    break;

                default:
                    _remoteEventsSender.Send(ToServerEvent.LoadUserpanelData, (int)type);
                    break;
            }

            _settingsHandler.RevertTempSettings();
        }

        #endregion Private Methods
    }
}
