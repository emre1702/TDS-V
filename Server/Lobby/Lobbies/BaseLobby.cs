using System.Threading.Tasks;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Sync;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class BaseLobby
    {
        internal readonly LobbyDb Entity;

        protected BaseLobbyDatabase Database { get; set; }
        public BaseLobbyEventsHandler Events { get; set; }
        protected BaseLobbyMapHandler MapHandler { get; set; }
        protected BaseLobbySync Sync { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobby(LobbyDb entity, BaseLobbyDatabase database, BaseLobbyEventsHandler events)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Entity = entity;

            Database = database;
            Database.SetLobby(this);

            Events = events;

            InitDependencies(entity, events);
            InitEvents(events);
        }

        protected virtual void InitDependencies(LobbyDb entity, BaseLobbyEventsHandler eventsHandler,
            BaseLobbyMapHandler? mapHandler = null, BaseLobbySync? sync = null)
        {
            MapHandler = mapHandler ?? new BaseLobbyMapHandler(entity);
            Sync = sync ?? new BaseLobbySync(entity, eventsHandler);
        }

        protected virtual void InitEvents(BaseLobbyEventsHandler eventsHandler)
        {
        }

        protected virtual async Task Remove(BaseLobby _)
        {
        }
    }
}
