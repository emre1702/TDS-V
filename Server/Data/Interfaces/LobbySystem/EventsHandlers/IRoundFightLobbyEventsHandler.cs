using System.Threading.Tasks;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Utility;

namespace TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers
{
#nullable enable

    public interface IRoundFightLobbyEventsHandler : IBaseLobbyEventsHandler
    {
        public delegate void RoundStateChangedDelegate();

        public delegate MapDto? RequestNewMapDelegate();

        public delegate void InitNewMapDelegate(MapDto mapDto);

        event RequestNewMapDelegate? RequestNewMap;

        event InitNewMapDelegate? InitNewMap;

        event RoundStateChangedDelegate? TeamPreparation;

        event RoundStateChangedDelegate? WeaponsLoading;

        event RoundStateChangedDelegate? PlayersPreparation;

        event RoundStateChangedDelegate? Countdown;

        event RoundStateChangedDelegate? InRound;

        AsyncValueTaskEvent? RoundEnd { get; set; }

        AsyncValueTaskEvent? RoundEndStats { get; set; }

        event RoundStateChangedDelegate? RoundEndRanking;

        AsyncValueTaskEvent? RoundClear { get; set; }

        void TriggerNewMapChoose();

        void TriggerTeamPreparation();

        void TriggerWeaponsLoading();

        void TriggerPlayersPreparation();

        void TriggerCountdown();

        void TriggerInRound();

        ValueTask TriggerRoundEnd();

        void TriggerRoundEndRanking();

        ValueTask TriggerRoundClear();

        ValueTask TriggerRoundEndStats();
    }
}
