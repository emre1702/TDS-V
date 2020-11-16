using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;

namespace TDS.Client.Handler
{
    public class FreeroamHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;

        public FreeroamHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, BrowserHandler browserHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            switch (settings.Type)
            {
                case LobbyType.MapCreateLobby:
                case LobbyType.GangLobby:
                    _browserHandler.Angular.ToggleFreeroam(false);
                    break;
            }
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            switch (settings.Type)
            {
                case LobbyType.MapCreateLobby:
                case LobbyType.GangLobby:
                    _browserHandler.Angular.ToggleFreeroam(true);
                    break;
            }
        }
    }
}
