using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Interfaces.LobbySystem;
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
    public class BaseLobby : INewLobby
    {
        public LobbyDb Entity { get; }

        protected BaseLobbyDatabase Database { get; set; }
        public BaseLobbyEventsHandler Events { get; set; }
        protected BaseLobbyMapHandler MapHandler { get; set; }
        protected BaseLobbyPlayers Players { get; set; }
        protected BaseLobbyTeamsHandler Teams { get; set; }
        protected BaseLobbySync Sync { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobby(LobbyDb entity, DatabaseHandler databaseHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Entity = entity;

            InitDependencies(databaseHandler);
            InitEvents();
        }

        protected virtual void InitDependencies(DatabaseHandler databaseHandler,
            BaseLobbyDatabase? database = null,
            BaseLobbyEventsHandler? events = null,
            BaseLobbyMapHandler? mapHandler = null,
            BaseLobbySync? sync = null,
            BaseLobbyPlayers? players = null,
            BaseLobbyTeamsHandler? teams = null)
        {
            Events = events ?? new BaseLobbyEventsHandler();
            Database = database ?? new BaseLobbyDatabase(this, databaseHandler, Events);
            MapHandler = mapHandler ?? new BaseLobbyMapHandler(Entity);
            Teams = teams ?? new BaseLobbyTeamsHandler(Entity);
            Players = players ?? new BaseLobbyPlayers(Entity, Events, Teams);

            Sync = sync ?? new BaseLobbySync(Entity, Events);
        }

        protected virtual void InitEvents()
        {
            Events.PlayerLeftLobby += async _ => await RemoveLobbyIfNeeded();
        }

        protected virtual async ValueTask RemoveLobbyIfNeeded()
        {
            if (!Entity.IsTemporary)
                return;

            if (Players.Any())
                return;

            await Events.TriggerLobbyRemove(this);
        }

        #region Operators

        public static bool operator !=(BaseLobby? lobby1, BaseLobby? lobby2)
        {
            return !(lobby1 == lobby2);
        }

        public static bool operator ==(BaseLobby? lobby1, BaseLobby? lobby2)
        {
            if (lobby1 is null)
                return lobby2 is null;
            if (lobby2 is null)
                return false;
            return lobby1.Entity.Id == lobby2.Entity.Id;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is INewLobby otherLobby))
                return false;
            return Entity.Id == otherLobby.Entity.Id;
        }

        public bool Equals(INewLobby? lobby)
        {
            if (lobby is null)
                return false;
            return Entity.Id == lobby.Entity.Id;
        }

        public override int GetHashCode()
            => Entity.Id;

        #endregion Operators
    }
}
