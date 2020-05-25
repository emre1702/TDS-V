using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class DiscordHandler : ServiceBase
    {
        #region Private Fields

        private string _lastLobbyName = "Login/Register";
        private string _lastTeamName = "Spectator";

        #endregion Private Fields

        #region Public Constructors

        public DiscordHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler) : base(modAPI, loggingHandler)
        {
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.TeamChanged += EventsHandler_TeamChanged;
        }

        #endregion Public Constructors

        #region Private Methods

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
            ModAPI.Discord.Update($"TDS-V - {lobbyName}", teamName);
        }

        #endregion Private Methods
    }
}
