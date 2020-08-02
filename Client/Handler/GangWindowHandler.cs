using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class GangWindowHandler : ServiceBase
    {
        #region Private Fields

        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;
        private readonly LobbyHandler _lobbyHandler;

        private bool _open;

        #endregion Private Fields

        #region Public Constructors

        public GangWindowHandler(IModAPI modAPI, LoggingHandler loggingHandler, BrowserHandler browserHandler, CursorHandler cursorHandler, SettingsHandler settingsHandler,
            RemoteEventsSender remoteEventsSender, Serializer serializer, EventsHandler eventsHandler, BindsHandler bindsHandler, LobbyHandler lobbyHandler)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _cursorHandler = cursorHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _serializer = serializer;
            _bindsHandler = bindsHandler;
            _lobbyHandler = lobbyHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;

            modAPI.Event.Add(FromBrowserEvent.CloseGangWindow, _ => Close());
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
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type == LobbyType.GangLobby)
                _bindsHandler.Add(Key.F3, Toggle);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type == LobbyType.GangLobby)
                _bindsHandler.Remove(Key.F3, Toggle);
        }


        #endregion Private Methods
    }
}
