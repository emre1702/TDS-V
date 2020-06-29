using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
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
        private SyncedPlayerSettingsDto _settings;

        #endregion Private Fields

        #region Public Constructors

        public InfosHandler(IModAPI modAPI, LoggingHandler loggingHandler, AngularBrowserHandler angularBrowserHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _angularBrowserHandler = angularBrowserHandler;

            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
            eventsHandler.CursorToggled += EventsHandler_CursorToggled;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EventsHandler_CursorToggled(bool boolean)
        {
            _cursorToggled = boolean;

            if (!(_settings is null))
            {
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, boolean);
            }
        }

        private void EventsHandler_SettingsLoaded(SyncedPlayerSettingsDto settings)
        {
            _settings = settings;

            if (_settings?.ShowCursorInfo == true)
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, _cursorToggled);
            else
                _angularBrowserHandler.ToggleInfo(InfoType.Cursor, false);

            if (_settings?.ShowLobbyLeaveInfo == true)
                _angularBrowserHandler.ToggleInfo(InfoType.LobbyLeave, true);
            else
                _angularBrowserHandler.ToggleInfo(InfoType.LobbyLeave, false);
        }

        #endregion Private Methods
    }
}
