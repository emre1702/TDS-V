using TDS_Server.Handler;
using TDS_Server.LobbySystem.EventsHandlers;

namespace TDS_Server.LobbySystem.Database
{
    public class ArenaDatabase : BaseLobbyDatabase
    {
        public ArenaDatabase(DatabaseHandler dbHandler, BaseLobbyEventsHandler eventsHandler) : base(dbHandler, eventsHandler)
        {
        }
    }
}
