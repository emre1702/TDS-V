using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Sync;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class Arena : BaseLobby
    {
        protected new ArenaDatabase Database => (ArenaDatabase)base.Database;
        protected new ArenaLobbySync Sync => (ArenaLobbySync)base.Sync;

        public Arena(LobbyDb entity, ArenaDatabase database, BaseLobbyEventsHandler eventsHandler) : base(entity, database, eventsHandler)
        {
        }

        protected override void InitDependencies(LobbyDb entity, BaseLobbyEventsHandler eventsHandler,
            BaseLobbyMapHandler? mapHandler = null, BaseLobbySync? sync = null)
        {
            base.InitDependencies(entity, eventsHandler, sync: new ArenaLobbySync(entity));
        }
    }
}
