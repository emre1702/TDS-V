using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Natives;
using TDS_Server.LobbySystem.Notifications;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sounds;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Models
{
    public class LobbyDependencies
    {
        public BaseLobbyChat? Chat { get; set; }
        public BaseLobbyDatabase? Database { get; set; }
        public IBaseLobbyEventsHandler? Events { get; set; }
        public BaseLobbyMapHandler? MapHandler { get; set; }
        public BaseLobbyNatives? Natives { get; set; }
        public BaseLobbyNotifications? Notifications { get; set; }
        public BaseLobbyPlayers? Players { get; set; }
        public BaseLobbySoundsHandler? Sounds { get; set; }
        public BaseLobbySync? Sync { get; set; }
        public BaseLobbyTeamsHandler? Teams { get; set; }
    }
}
