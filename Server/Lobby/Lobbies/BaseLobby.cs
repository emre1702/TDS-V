using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Models;
using TDS_Server.LobbySystem.Natives;
using TDS_Server.LobbySystem.Notifications;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sounds;
using TDS_Server.LobbySystem.Sync;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public abstract class BaseLobby : IBaseLobby
    {
        public LobbyDb Entity { get; }

        protected BaseLobbyChat Chat { get; private set; }
        protected BaseLobbyDatabase Database { get; private set; }
        public IBaseLobbyEventsHandler Events { get; private set; }
        protected BaseLobbyMapHandler MapHandler { get; private set; }
        protected BaseLobbyNatives Natives { get; private set; }
        protected BaseLobbyNotifications Notifications { get; private set; }
        protected BaseLobbyPlayers Players { get; private set; }
        protected BaseLobbyTeamsHandler Teams { get; private set; }
        protected BaseLobbySoundsHandler Sounds { get; private set; }
        protected BaseLobbySync Sync { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Entity = entity;

            InitDependencies(databaseHandler, langHelper, eventsHandler);
            InitEvents();
        }

        protected virtual void InitDependencies(
            DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            LobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new LobbyDependencies();

            Events = lobbyDependencies.Events ?? new BaseLobbyEventsHandler(eventsHandler, this);
            Database = lobbyDependencies.Database ?? new BaseLobbyDatabase(this, databaseHandler, Events);
            MapHandler = lobbyDependencies.MapHandler ?? new BaseLobbyMapHandler(Entity);
            uint dimensionProvider() => MapHandler.Dimension;
            Natives = lobbyDependencies.Natives ?? new BaseLobbyNatives(dimensionProvider);
            Teams = lobbyDependencies.Teams ?? new BaseLobbyTeamsHandler(Entity);
            Players = lobbyDependencies.Players ?? new BaseLobbyPlayers(Entity, Events, Teams);
            Chat = lobbyDependencies.Chat ?? new BaseLobbyChat(Players, langHelper);
            Notifications = lobbyDependencies.Notifications ?? new BaseLobbyNotifications(Players, langHelper);
            Sync = lobbyDependencies.Sync ?? new BaseLobbySync(Entity, Events, dimensionProvider);
            Sounds = lobbyDependencies.Sounds ?? new BaseLobbySoundsHandler(Sync);
        }

        protected virtual void InitEvents()
        {
            Events.PlayerLeftLobby += async _ => await RemoveLobbyIfNeeded();
        }

        protected virtual async ValueTask RemoveLobbyIfNeeded()
        {
            if (!Entity.IsTemporary)
                return;

            if (await Players.Any())
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
            if (!(obj is IBaseLobby otherLobby))
                return false;
            return Entity.Id == otherLobby.Entity.Id;
        }

        public bool Equals(IBaseLobby? lobby)
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
