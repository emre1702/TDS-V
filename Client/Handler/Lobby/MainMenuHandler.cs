using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;

namespace TDS.Client.Handler.Lobby
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

            RAGE.Game.Ui.DisplayRadar(false);
            _browserHandler.Angular.ToggleLobbyChoiceMenu(true);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.MainMenu)
                return;

            RAGE.Game.Ui.DisplayRadar(true);
            _browserHandler.Angular.ToggleLobbyChoiceMenu(false);
        }
    }
}
