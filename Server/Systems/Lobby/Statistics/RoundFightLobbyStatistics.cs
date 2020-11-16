using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Statistics;

namespace TDS.Server.LobbySystem.Statistics
{
    public class RoundFightLobbyStatistics : IRoundFightLobbyStatistics
    {
        protected IRoundFightLobby Lobby { get; }
        private readonly IRoundFightLobbyEventsHandler _events;

        public RoundFightLobbyStatistics(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
        {
            Lobby = lobby;
            _events = events;

            events.RoundEndStats += RoundEndStats;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            if (_events.RoundEndStats is { })
                _events.RoundEndStats -= RoundEndStats;
            _events.RemoveAfter -= RemoveEvents;
        }

        private async ValueTask RoundEndStats()
        {
            await Lobby.Database.Save().ConfigureAwait(false);
        }
    }
}
