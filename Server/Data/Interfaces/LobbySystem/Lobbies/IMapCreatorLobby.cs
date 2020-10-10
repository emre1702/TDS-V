using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Sync;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IMapCreatorLobby : IFreeroamLobby
    {
        new IMapCreatorLobbySync Sync { get; }
    }
}
