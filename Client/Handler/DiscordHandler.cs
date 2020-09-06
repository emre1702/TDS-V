using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class DiscordHandler : ServiceBase
    {
        private string _lastLobbyName = "Login/Register";
        private string _lastTeamName = "Spectator";

        public DiscordHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler) : base(loggingHandler)
        {
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.TeamChanged += EventsHandler_TeamChanged;
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            _lastLobbyName = settings.Name;
            Update(_lastLobbyName, _lastTeamName);
        }

        private void EventsHandler_TeamChanged(string teamName)
        {
            _lastTeamName = teamName;
            Update(_lastTeamName, _lastTeamName);
        }

        private void Update(string lobbyName, string teamName)
        {
            RAGE.Discord.Update($"TDS-V - {lobbyName}", teamName);
        }
    }
}
