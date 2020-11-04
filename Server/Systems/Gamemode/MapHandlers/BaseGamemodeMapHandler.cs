using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS_Server.GamemodesSystem.MapHandlers
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
