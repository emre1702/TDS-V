using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.LobbySystem.MapHandlers;

namespace TDS.Server.Data.Interfaces.LobbySystem.Lobbies
{
    public interface IArena : IRoundFightLobby
    {
        new IArenaMapHandler MapHandler { get; }
        IArenaMapVoting MapVoting { get; }
        new IArenaRoundsHandler Rounds { get; }
        new IArenaTeamsHandler Teams { get; }
    }
}
