using TDS_Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Chat;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Notifications;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IBaseLobby
    {
        public LobbyType Type => Entity.Type;
        public bool IsOfficial => Entity.IsOfficial;
        public bool IsRemoved => Events.IsRemoved;

        LobbyDb Entity { get; }

        IBaseLobbyBansHandler Bans { get; }
        IBaseLobbyChat Chat { get; }
        IBaseLobbyColshapesHandler ColshapesHandler { get; }
        IBaseLobbyDatabase Database { get; }
        IBaseLobbyDeathmatch Deathmatch { get; }
        IBaseLobbyEventsHandler Events { get; }
        IBaseLobbyMapHandler MapHandler { get; }
        IBaseLobbyNotifications Notifications { get; }
        IBaseLobbyPlayers Players { get; }
        IBaseLobbyTeamsHandler Teams { get; }
    }
}
