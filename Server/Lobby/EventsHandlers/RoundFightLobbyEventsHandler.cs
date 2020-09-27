using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Events;
using static TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers.IRoundFightLobbyEventsHandler;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class RoundFightLobbyEventsHandler : BaseLobbyEventsHandler, IRoundFightLobbyEventsHandler
    {
        public event RequestNewMapDelegate? RequestNewMap;

        public event InitNewMapDelegate? InitNewMap;

        public event RoundStateChangedDelegate? TeamPreparation;

        public event RoundStateChangedDelegate? WeaponsLoading;

        public event RoundStateChangedDelegate? PlayersPreparation;

        public event RoundStateChangedDelegate? Countdown;

        public event RoundStateChangedDelegate? InRound;

        public AsyncValueTaskEvent? RoundEnd { get; set; }

        public AsyncValueTaskEvent? RoundEndStats { get; set; }

        public AsyncValueTaskEvent? RoundEndRanking { get; set; }

        public AsyncValueTaskEvent? RoundClear { get; set; }

        public RoundFightLobbyEventsHandler(EventsHandler eventsHandler, IRoundFightLobby lobby, ILoggingHandler logging)
            : base(eventsHandler, lobby, logging)
        {
        }

        public void TriggerNewMapChoose()
        {
            var mapDto = RequestNewMap?.Invoke();
            if (mapDto is null)
            {
                Logging.LogError("RequestNewMap didn't return a map.");
                return;
            }

            InitNewMap?.Invoke(mapDto);
        }

        public void TriggerTeamPreparation()
            => TeamPreparation?.Invoke();

        public void TriggerWeaponsLoading()
            => WeaponsLoading?.Invoke();

        public void TriggerPlayersPreparation()
            => PlayersPreparation?.Invoke();

        public void TriggerCountdown()
            => Countdown?.Invoke();

        public void TriggerInRound()
            => InRound?.Invoke();

        public ValueTask TriggerRoundClear()
            => RoundClear?.InvokeAsync() ?? default;

        public ValueTask TriggerRoundEnd()
            => RoundEnd?.InvokeAsync() ?? default;

        public ValueTask TriggerRoundEndRanking()
            => RoundEndRanking?.InvokeAsync() ?? default;

        public ValueTask TriggerRoundEndStats()
            => RoundEndStats?.InvokeAsync() ?? default;
    }
}
