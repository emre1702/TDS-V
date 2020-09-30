using TDS_Server.Data.Interfaces.GangSystem.GangGamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangActionLobby : IRoundFightLobby
    {
        IGangArea GangArea { get; }
        new IGangActionLobbyTeamsHandler Teams { get; }
    }
}
