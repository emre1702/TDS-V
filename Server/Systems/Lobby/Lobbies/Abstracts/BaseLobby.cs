using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.BansHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Chat;
using TDS.Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS.Server.Data.Interfaces.LobbySystem.Database;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Natives;
using TDS.Server.Data.Interfaces.LobbySystem.Notifications;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Interfaces.LobbySystem.Sounds;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.Chats;
using TDS.Server.LobbySystem.ColshapesHandlers;
using TDS.Server.LobbySystem.Database;
using TDS.Server.LobbySystem.Deathmatch;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Natives;
using TDS.Server.LobbySystem.Notifications;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.Sounds;
using TDS.Server.LobbySystem.Sync;
using TDS.Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class BaseLobby : Data.Interfaces.LobbySystem.Lobbies.Abstracts.IBaseLobby
    {
        public LobbyDb Entity { get; }

        public IBaseLobbyBansHandler Bans { get; private set; }
        public IBaseLobbyChat Chat { get; private set; }
        public IBaseLobbyColshapesHandler ColshapesHandler { get; private set; }
        public IBaseLobbyDatabase Database { get; private set; }
        public IBaseLobbyDeathmatch Deathmatch { get; private set; }
        public IDatabaseHandler GlobalDatabaseHandler { get; }
        public IBaseLobbyEventsHandler Events { get; private set; }
        public EventsHandler GlobalEventsHandler { get; }
        public LangHelper LangHelper { get; }
        protected ILoggingHandler LoggingHandler { get; }
        protected LobbiesHandler LobbiesHandler { get; }
        protected RemoteBrowserEventsHandler RemoteBrowserEventsHandler { get; }
        public IBaseLobbyMapHandler MapHandler { get; private set; }
        public IBaseLobbyNatives Natives { get; private set; }
        public IBaseLobbyNotifications Notifications { get; private set; }
        public IBaseLobbyPlayers Players { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public IBaseLobbySoundsHandler Sounds { get; private set; }
        public IBaseLobbySync Sync { get; private set; }
        public IBaseLobbyTeamsHandler Teams { get; private set; }
        public ITeamsProvider TeamsProvider { get; }

        public TaskCompletionSource<bool> IsCreatingTask { get; } = new TaskCompletionSource<bool>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected BaseLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Entity = entity;
            GlobalEventsHandler = eventsHandler;
            LangHelper = langHelper;
            GlobalDatabaseHandler = databaseHandler;
            ServiceProvider = serviceProvider;
            TeamsProvider = teamsProvider;
            LoggingHandler = loggingHandler;
            LobbiesHandler = lobbiesHandler;
            RemoteBrowserEventsHandler = remoteBrowserEventsHandler;

            InitDependencies();

            Events!.PlayerLeft += PlayerLeft;
        }

        protected virtual async void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            try
            {
                lobbyDependencies ??= new BaseLobbyDependencies();

                lobbyDependencies.Events ??= new BaseLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
                lobbyDependencies.Bans ??= new BaseLobbyBansHandler(this, LangHelper);
                lobbyDependencies.Chat ??= new BaseLobbyChat(this, LangHelper);
                lobbyDependencies.ColshapesHandler ??= new BaseLobbyColshapesHandler();
                lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(this, lobbyDependencies.Events, TeamsProvider);
                lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);
                lobbyDependencies.Deathmatch ??= new BaseLobbyDeathmatch(this, lobbyDependencies.Events);
                lobbyDependencies.MapHandler ??= new BaseLobbyMapHandler(this, lobbyDependencies.Events);
                lobbyDependencies.Natives ??= new BaseLobbyNatives(this);
                lobbyDependencies.Notifications ??= new BaseLobbyNotifications(this, LangHelper);
                lobbyDependencies.Players ??= new BaseLobbyPlayers(this, lobbyDependencies.Events);

                lobbyDependencies.Sync ??= new BaseLobbySync(this, lobbyDependencies.Events);
                lobbyDependencies.Sounds ??= new BaseLobbySoundsHandler(this);

                Bans = lobbyDependencies.Bans;
                Chat = lobbyDependencies.Chat;
                ColshapesHandler = lobbyDependencies.ColshapesHandler;
                Database = lobbyDependencies.Database;
                Deathmatch = lobbyDependencies.Deathmatch;
                Events = lobbyDependencies.Events;
                MapHandler = lobbyDependencies.MapHandler;
                Natives = lobbyDependencies.Natives;
                Notifications = lobbyDependencies.Notifications;
                Players = lobbyDependencies.Players;
                Sounds = lobbyDependencies.Sounds;
                Sync = lobbyDependencies.Sync;
                Teams = lobbyDependencies.Teams;

                await Events.TriggerCreated(Entity).ConfigureAwait(false);
                IsCreatingTask.SetResult(true);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex);
            }
        }

        protected virtual async ValueTask CheckRemoveLobby()
        {
            if (!Entity.IsTemporary)
                return;

            if (await Players.Any().ConfigureAwait(false))
                return;

            await Remove().ConfigureAwait(false);
        }

        public virtual async Task Remove()
        {
            if (Events.PlayerLeft is { })
                Events.PlayerLeft -= PlayerLeft;
            await Events.TriggerRemove().ConfigureAwait(false);
        }

        private async ValueTask PlayerLeft((ITDSPlayer Player, int HadLifes) data)
        {
            await CheckRemoveLobby().ConfigureAwait(false);
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
            if (obj is not Data.Interfaces.LobbySystem.Lobbies.Abstracts.IBaseLobby otherLobby)
                return false;
            return Entity.Id == otherLobby.Entity.Id;
        }

        public bool Equals(Data.Interfaces.LobbySystem.Lobbies.Abstracts.IBaseLobby? lobby)
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