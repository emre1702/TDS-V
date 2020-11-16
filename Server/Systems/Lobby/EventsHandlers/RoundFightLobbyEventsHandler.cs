using System;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Events;
using static TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers.IRoundFightLobbyEventsHandler;

namespace TDS.Server.LobbySystem.EventsHandlers
{
    public class RoundFightLobbyEventsHandler : FightLobbyEventsHandler, IRoundFightLobbyEventsHandler
    {
        public event RequestNewMapDelegate? RequestNewMap;

        public event InitNewMapDelegate? RequestGamemode;

        public event InitNewMapDelegate? InitNewMap;

        public event RoundStateChangedDelegate? TeamPreparation;

        public event RoundStateChangedDelegate? WeaponsLoading;

        public event RoundStateChangedDelegate? PlayersPreparation;

        public event RoundStateChangedDelegate? Countdown;

        public AsyncValueTaskEvent? InRound { get; set; }

        public AsyncValueTaskEvent? RoundEnd { get; set; }

        public AsyncValueTaskEvent? RoundEndStats { get; set; }

        public AsyncValueTaskEvent? RoundEndRanking { get; set; }

        public AsyncValueTaskEvent? RoundClear { get; set; }

        public RoundFightLobbyEventsHandler(IRoundFightLobby lobby, EventsHandler eventsHandler, ILoggingHandler loggingHandler)
            : base(lobby, eventsHandler, loggingHandler)
        {
        }

        public void TriggerNewMapChoose()
        {
            try
            {
                var mapDto = RequestNewMap?.Invoke();
                if (mapDto is null)
                {
                    Handler.LoggingHandler.Instance.LogError("RequestNewMap didn't return a map.");
                    return;
                }

                InitNewMap?.Invoke(mapDto);
                RequestGamemode?.Invoke(mapDto);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerTeamPreparation()
        {
            try
            {
                TeamPreparation?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerWeaponsLoading()
        {
            try
            {
                WeaponsLoading?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerPlayersPreparation()
        {
            try
            {
                PlayersPreparation?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public void TriggerCountdown()
        {
            try
            {
                Countdown?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        public ValueTask TriggerInRound()
        {
            try
            {
                return InRound?.InvokeAsync() ?? default;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
                return default;
            }
        }

        public ValueTask TriggerRoundClear()
        {
            try
            {
                return RoundClear?.InvokeAsync() ?? default;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
                return default;
            }
        }

        public ValueTask TriggerRoundEnd()
        {
            try
            {
                return RoundEnd?.InvokeAsync() ?? default;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
                return default;
            }
        }

        public ValueTask TriggerRoundEndRanking()
        { 
            try
            {
                return RoundEndRanking?.InvokeAsync() ?? default;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
                return default;
            }
        }

        public ValueTask TriggerRoundEndStats()
        {
            try
            {
                return RoundEndStats?.InvokeAsync() ?? default;
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
                return default;
            }
        }

    }
}
