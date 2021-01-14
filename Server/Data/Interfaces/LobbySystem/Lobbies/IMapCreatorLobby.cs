using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;
using TDS.Server.LobbySystem.MapHandlers;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IMapCreatorLobby : IFreeroamLobby
    {
        new IMapCreatorMapHandler MapHandler { get; }
        new IMapCreatorLobbySync Sync { get; }
    }
}