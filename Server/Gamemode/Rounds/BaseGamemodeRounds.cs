using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;

namespace TDS_Server.GamemodesSystem.Rounds
{
    public class BaseGamemodeRounds : IBaseGamemodeRounds
    {
        private IRoundFightLobbyEventsHandler? _events;

        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            _events = events;
            events.RoundClear += RoundClear;
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            if (events.RoundClear is { })
                events.RoundClear -= RoundClear;
        }

        public virtual bool CanEndRound(IRoundEndReason roundEndReason) => true;

        public virtual bool CanJoinDuringRound(ITDSPlayer player, ITeam team) => false;

        public virtual void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
        }

        protected virtual ValueTask RoundClear()
        {
            if (_events is { })
                RemoveEvents(_events);
            return default;
        }
    }
}
