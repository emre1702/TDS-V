using TDS_Server.Data.Interfaces.GamemodesSystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;

namespace TDS_Server.GamemodesSystem.Deathmatch
{
    public class BaseGamemodeDeathmatch : IBaseGamemodeDeathmatch
    {
        internal virtual void AddEvents(IRoundFightLobbyEventsHandler events)
        {
        }

        internal virtual void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
        }
    }
}
