using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GamemodesSystem.Rounds;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.GamemodesSystem.Rounds
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

        /// <summary>In GangAction lobby this isn't checked</summary> ///
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
