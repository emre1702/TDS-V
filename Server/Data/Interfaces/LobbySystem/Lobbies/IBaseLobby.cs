using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
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
        IBaseLobbyEventsHandler Events { get; }
    }
}
