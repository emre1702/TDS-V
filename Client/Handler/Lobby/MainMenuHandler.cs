using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.Lobby
{
    public class MainMenuHandler
    {
        private readonly BrowserHandler _browserHandler;

        public MainMenuHandler(EventsHandler eventsHandler, BrowserHandler browserHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

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
    }
}
