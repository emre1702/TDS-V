using TDS_Server.Data.Interfaces.Entities.Gangs;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangActionLobby : IRoundFightLobby
    {
        IGangwarArea GangArea { get; }
        new IGangActionLobbyTeamsHandler Teams { get; }
    }
}
