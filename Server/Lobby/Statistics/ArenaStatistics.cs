using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS_Server.LobbySystem.Statistics
{
    public class ArenaStatistics : RoundFightLobbyStatistics
    {
        public ArenaStatistics(IRoundFightLobby lobby) : base(lobby)
        {
        }
    }
}
