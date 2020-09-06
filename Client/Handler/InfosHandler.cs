using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class InfosHandler : ServiceBase
    {
        #region Private Fields

        private readonly AngularBrowserHandler _angularBrowserHandler;

        private bool _cursorToggled;
        private bool _inMainMenu = true;
        private SyncedPlayerSettingsDto _settings;

        #endregion Private Fields

        #region Public Constructors

        public InfosHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _angularBrowserHandler = browserHandler.Angular;

            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
            eventsHandler.CursorToggled += EventsHandler_CursorToggled;
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EventsHandler_CursorToggled(bool boolean)
        {
            _cursorToggled = boolean;

            if (_settings?.ShowCursorInfo == true)
            {
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, boolean);
            }
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            _inMainMenu = settings.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu;

            if (_settings?.ShowLobbyLeaveInfo == true)
                _angularBrowserHandler.ToggleInfo(InfoType.LobbyLeave, !_inMainMenu);
        }

        private void EventsHandler_SettingsLoaded(SyncedPlayerSettingsDto settings)
        {
            _settings = settings;

            if (_settings?.ShowCursorInfo == true)
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, _cursorToggled);
            else
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, false);

            if (_settings?.ShowLobbyLeaveInfo == true)
                _angularBrowserHandler.ToggleInfo(InfoType.LobbyLeave, !_inMainMenu);
            else
                _angularBrowserHandler.ToggleInfo(InfoType.LobbyLeave, false);
        }

        #endregion Private Methods
    }
}
