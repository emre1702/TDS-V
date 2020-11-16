using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;

namespace TDS.Client.Handler
{
    public class GangWindowHandler : ServiceBase
    {
        #region Private Fields

        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;

        private bool _open;

        #endregion Private Fields

        #region Public Constructors

        public GangWindowHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, CursorHandler cursorHandler,
            EventsHandler eventsHandler, BindsHandler bindsHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _cursorHandler = cursorHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;

            RAGE.Events.Add(FromBrowserEvent.CloseGangWindow, _ => Close());
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
            _browserHandler.Angular.ToggleGangWindow(_open);
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
