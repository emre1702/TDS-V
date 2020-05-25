using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.Lobby
{
    public class MainMenuHandler
    {
        #region Private Fields

        private readonly BrowserHandler _browserHandler;

        #endregion Private Fields

        #region Public Constructors

        public MainMenuHandler(EventsHandler eventsHandler, BrowserHandler browserHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.MainMenu)
                return;

            _browserHandler.Angular.ToggleLobbyChoiceMenu(true);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.MainMenu)
                return;

            _browserHandler.Angular.ToggleLobbyChoiceMenu(false);
        }

        #endregion Private Methods
    }
}
