using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class DiscordHandler
    {
        private string _lastLobbyName = "Login/Register";
        private string _lastTeamName = "Spectator";

        private readonly IModAPI _modAPI;

        public DiscordHandler(IModAPI modAPI, EventsHandler eventsHandler)
        {
            _modAPI = modAPI;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.TeamChanged += EventsHandler_TeamChanged;
        }

        private void Update(string lobbyName, string teamName)
        {
            _modAPI.Discord.Update($"TDS-V - {lobbyName}", teamName);
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
    }
}
