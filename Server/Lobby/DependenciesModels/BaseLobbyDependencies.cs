using TDS_Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Chat;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Natives;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.Sounds;
using TDS_Server.Data.Interfaces.LobbySystem.Sync;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.LobbySystem.Vehicles;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    public class BaseLobbyDependencies
    {
        public IBaseLobbyBansHandler? Bans { get; set; }
        public IBaseLobbyChat? Chat { get; set; }
        public IBaseLobbyColshapesHandler? ColshapesHandler { get; set; }
        public IBaseLobbyDatabase? Database { get; set; }
        public IBaseLobbyDeathmatch? Deathmatch { get; set; }
        public IBaseLobbyEventsHandler? Events { get; set; }
        public IBaseLobbyMapHandler? MapHandler { get; set; }
        public IBaseLobbyNatives? Natives { get; set; }
        public IBaseLobbyNotifications? Notifications { get; set; }
        public IBaseLobbyPlayers? Players { get; set; }
        public IBaseLobbySoundsHandler? Sounds { get; set; }
        public IBaseLobbySync? Sync { get; set; }
        public IBaseLobbyTeamsHandler? Teams { get; set; }
        public BaseLobbyVehicles? Vehicles { get; set; }
    }
}
