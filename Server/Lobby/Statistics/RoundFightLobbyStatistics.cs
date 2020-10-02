using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Statistics;

namespace TDS_Server.LobbySystem.Statistics
{
    public class RoundFightLobbyStatistics : IRoundFightLobbyStatistics
    {
        protected IRoundFightLobby Lobby { get; }

        public RoundFightLobbyStatistics(IRoundFightLobby lobby)
        {
            Lobby = lobby;
            lobby.Events.RoundEndStats += RoundEndStats;
        }

        private async ValueTask RoundEndStats()
        {
            await Lobby.Database.Save();
        }
    }
}
