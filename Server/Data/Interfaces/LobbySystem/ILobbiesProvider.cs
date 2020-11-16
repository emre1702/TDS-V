using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.Data.Interfaces.LobbySystem
{
    public interface ILobbiesProvider
    {
        TLobby Create<TLobby>(ITDSPlayer owner) where TLobby : IBaseLobby;

        TLobby Create<TLobby>(LobbyDb entity) where TLobby : IBaseLobby;

        IBaseLobby Create(LobbyType lobbyType, LobbyDb entity);
    }
}
