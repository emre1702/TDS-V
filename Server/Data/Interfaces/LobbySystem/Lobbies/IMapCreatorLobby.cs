using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IMapCreatorLobby : IFreeroamLobby
    {
        new IMapCreatorLobbySync Sync { get; }
    }
}
