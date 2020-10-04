using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS_Server.GamemodesSystem.Players
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
