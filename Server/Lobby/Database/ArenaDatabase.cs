using TDS_Server.Handler;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.Database
{
    public class ArenaDatabase : BaseLobbyDatabase
    {
        public ArenaDatabase(Arena lobby, DatabaseHandler dbHandler, BaseLobbyEventsHandler eventsHandler) : base(lobby, dbHandler, eventsHandler)
        {
        }
    }
}
