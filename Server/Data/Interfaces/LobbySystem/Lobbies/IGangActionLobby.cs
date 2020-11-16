using TDS.Server.Data.Interfaces.Entities.Gangs;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangActionLobby : IRoundFightLobby
    {
        IGangwarArea GangArea { get; }
        new IGangActionLobbyTeamsHandler Teams { get; }
    }
}
