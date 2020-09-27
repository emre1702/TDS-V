using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IArena : IRoundFightLobby
    {
        IArenaMapVoting MapVoting { get; }
        new IArenaTeamsHandler Teams { get; }
    }
}
