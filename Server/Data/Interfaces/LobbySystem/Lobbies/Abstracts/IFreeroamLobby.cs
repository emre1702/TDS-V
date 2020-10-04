using TDS_Server.Data.Interfaces.LobbySystem.Freeroam;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IFreeroamLobby : IBaseLobby
    {
        IFreeroamLobbyFreeroam Freeroam { get; }
    }
}
