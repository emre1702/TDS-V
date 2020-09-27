using TDS_Server.Data.Interfaces.LobbySystem.Chat;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Natives;
using TDS_Server.LobbySystem.Sounds;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Models
{
    public class LobbyDependencies
    {
        public BaseLobbyBansHandler? Bans { get; set; }
        public IBaseLobbyChat? Chat { get; set; }
        public IBaseLobbyColshapesHandler? ColshapesHandler { get; set; }
        public IBaseLobbyDatabase? Database { get; set; }
        public IBaseLobbyDeathmatch? Deathmatch { get; set; }
        public IBaseLobbyEventsHandler? Events { get; set; }
        public IBaseLobbyMapHandler? MapHandler { get; set; }
        public BaseLobbyNatives? Natives { get; set; }
        public IBaseLobbyNotifications? Notifications { get; set; }
        public IBaseLobbyPlayers? Players { get; set; }
        public BaseLobbySoundsHandler? Sounds { get; set; }
        public BaseLobbySync? Sync { get; set; }
        public BaseLobbyTeamsHandler? Teams { get; set; }
    }
}
