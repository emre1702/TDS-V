using TDS.Server.Data.Interfaces.LobbySystem.Freeroam;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IFreeroamLobby : IBaseLobby
    {
        IFreeroamLobbyFreeroam Freeroam { get; }
    }
}
