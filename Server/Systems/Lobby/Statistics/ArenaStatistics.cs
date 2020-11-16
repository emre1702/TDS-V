using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.LobbySystem.Statistics
{
    public class ArenaStatistics : RoundFightLobbyStatistics
    {
        public ArenaStatistics(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
        }
    }
}
