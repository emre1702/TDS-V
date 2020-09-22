using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IBaseLobby
    {
        public LobbyType Type => Entity.Type;
        public bool IsOfficial => Entity.IsOfficial;
        public bool IsRemoved => Events.IsRemoved;

        LobbyDb Entity { get; }

        IBaseLobbyColshapesHandler ColshapesHandler { get; }
        IBaseLobbyDeathmatch Deathmatch { get; }
        IBaseLobbyEventsHandler Events { get; }
        IBaseLobbyMapHandler MapHandler { get; }
        IBaseLobbyPlayers Players { get; }
    }
}
