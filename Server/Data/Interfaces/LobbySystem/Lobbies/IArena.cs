using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.LobbySystem.MapHandlers;

namespace TDS_Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IArena : IRoundFightLobby
    {
        new IArenaMapHandler MapHandler { get; }
        IArenaMapVoting MapVoting { get; }
        new IArenaRoundsHandler Rounds { get; }
        new IArenaTeamsHandler Teams { get; }
    }
}
