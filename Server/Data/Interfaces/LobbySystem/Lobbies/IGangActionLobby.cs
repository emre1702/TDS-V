using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IGangActionLobby : IRoundFightLobby
    {
        IBaseGangActionArea GangArea { get; }
        new IGangActionLobbyTeamsHandler Teams { get; }
    }
}
