using TDS_Server.Data.Interfaces.LobbySystem.Spectator;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IFightLobby : IBaseLobby
    {
        IFightLobbySpectator Spectator { get; }
    }
}
