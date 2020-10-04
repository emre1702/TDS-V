using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.Statistics
{
    public class ArenaStatistics : RoundFightLobbyStatistics
    {
        public ArenaStatistics(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
        }
    }
}
