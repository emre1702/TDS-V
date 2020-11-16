using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS.Server.GamemodesSystem.MapHandlers
{
    public class BaseGamemodeMapHandler : IBaseGamemodeMapHandler
    {
        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
        }
    }
}
