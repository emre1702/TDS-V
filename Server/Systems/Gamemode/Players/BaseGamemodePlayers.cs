using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS.Server.GamemodesSystem.Players
{
    public class BaseGamemodePlayers : IBaseGamemodePlayers
    {
        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            events.PlayerJoinedAfter += PlayerJoinedAfter;
            events.PlayerLeftAfter += PlayerLeftAfter;
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            if (events.PlayerJoinedAfter is { })
                events.PlayerJoinedAfter -= PlayerJoinedAfter;
            if (events.PlayerLeftAfter is { })
                events.PlayerLeftAfter -= PlayerLeftAfter;
        }

        protected virtual ValueTask PlayerJoinedAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            return default;
        }

        protected virtual ValueTask PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            return default;
        }
    }
}
