using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem
{
    public interface ILobbiesProvider
    {
        TLobby Create<TLobby>(ITDSPlayer owner) where TLobby : IBaseLobby;

        TLobby Create<TLobby>(LobbyDb entity) where TLobby : IBaseLobby;

        IBaseLobby Create(LobbyType lobbyType, LobbyDb entity);
    }
}
