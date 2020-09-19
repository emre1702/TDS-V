using TDS_Server.Handler;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class Arena : BaseLobby
    {
        protected new ArenaDatabase Database => (ArenaDatabase)base.Database;
        protected new ArenaLobbySync Sync => (ArenaLobbySync)base.Sync;

        public Arena(LobbyDb entity, DatabaseHandler databaseHandler) : base(entity, databaseHandler)
        {
        }

        protected override void InitDependencies(DatabaseHandler databaseHandler,
            BaseLobbyDatabase? database = null,
            BaseLobbyEventsHandler? events = null,
            BaseLobbyMapHandler? mapHandler = null,
            BaseLobbySync? sync = null,
            BaseLobbyPlayers? players = null,
            BaseLobbyTeamsHandler? teams = null)
        {
            events ??= new BaseLobbyEventsHandler();

            base.InitDependencies(databaseHandler,
                events: events,
                sync: new ArenaLobbySync(Entity, events));
        }
    }
}
