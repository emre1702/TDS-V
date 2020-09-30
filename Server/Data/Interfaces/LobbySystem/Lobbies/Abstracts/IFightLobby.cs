using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts
{
    public interface IFightLobby : IBaseLobby
    {
        new IFightLobbyDeathmatch Deathmatch { get; }
        IFightLobbySpectator Spectator { get; }
        new IFightLobbyTeamsHandler Teams { get; }
    }
}
