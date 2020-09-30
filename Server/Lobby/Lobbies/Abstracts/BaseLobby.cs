﻿using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Chats;
using TDS_Server.LobbySystem.ColshapesHandlers;
using TDS_Server.LobbySystem.Database;
using TDS_Server.LobbySystem.Deathmatch;
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

namespace TDS_Server.LobbySystem.Lobbies.Abstracts
{
    public abstract class BaseLobby : IBaseLobby
    {
        public LobbyDb Entity { get; }

        public BaseLobbyBansHandler Bans { get; private set; }
        public BaseLobbyChat Chat { get; private set; }
        public IBaseLobbyColshapesHandler ColshapesHandler { get; private set; }
        public IBaseLobbyDatabase Database { get; private set; }
        public IBaseLobbyDeathmatch Deathmatch { get; private set; }
        public DatabaseHandler GlobalDatabaseHandler { get; }
        public IBaseLobbyEventsHandler Events { get; private set; }
        public EventsHandler GlobalEventsHandler { get; }
        public LangHelper LangHelper { get; }
        public IBaseLobbyMapHandler MapHandler { get; private set; }
        public BaseLobbyNatives Natives { get; private set; }
        public BaseLobbyNotifications Notifications { get; private set; }
        public IBaseLobbyPlayers Players { get; private set; }
        public BaseLobbyTeamsHandler Teams { get; private set; }
        public BaseLobbySoundsHandler Sounds { get; private set; }
        public BaseLobbySync Sync { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Entity = entity;
            GlobalEventsHandler = eventsHandler;
            LangHelper = langHelper;
            GlobalDatabaseHandler = databaseHandler;

            InitDependencies();

            Events.PlayerLeft += async _ => await CheckRemoveLobby();
        }

        protected virtual void InitDependencies(LobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new LobbyDependencies();

            uint dimensionProvider() => MapHandler.Dimension;
            void doForPlayersInMainThreadFuncProvider(Action<ITDSPlayer> action) => Players.DoInMain(action);

            lobbyDependencies.ColshapesHandler ??= new BaseLobbyColshapesHandler();
            lobbyDependencies.Natives ??= new BaseLobbyNatives(dimensionProvider);
            lobbyDependencies.Teams ??= new BaseLobbyTeamsHandler(Entity);
            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(GlobalEventsHandler, this);
            lobbyDependencies.MapHandler ??= new BaseLobbyMapHandler(Entity);

            lobbyDependencies.Deathmatch ??= new BaseLobbyDeathmatch(lobbyDependencies.Events, this);
            lobbyDependencies.Database ??= new BaseLobbyDatabase(this, GlobalDatabaseHandler, lobbyDependencies.Events);

            lobbyDependencies.Notifications ??= new BaseLobbyNotifications(doForPlayersInMainThreadFuncProvider, LangHelper);
            lobbyDependencies.Chat ??= new BaseLobbyChat(doForPlayersInMainThreadFuncProvider, LangHelper);
            lobbyDependencies.Bans ??= new BaseLobbyBansHandler(lobbyDependencies.Database, lobbyDependencies.Events, LangHelper, lobbyDependencies.Chat, Entity);
            lobbyDependencies.Players ??= new BaseLobbyPlayers(this, lobbyDependencies.Events, lobbyDependencies.Teams, lobbyDependencies.Bans);

            lobbyDependencies.Sync ??= new BaseLobbySync(Entity, lobbyDependencies.Events, dimensionProvider);
            lobbyDependencies.Sounds ??= new BaseLobbySoundsHandler(lobbyDependencies.Sync);

            Bans = lobbyDependencies.Bans;
            Chat = lobbyDependencies.Chat;
            Database = lobbyDependencies.Database;
            Deathmatch = lobbyDependencies.Deathmatch;
            Events = lobbyDependencies.Events;
            MapHandler = lobbyDependencies.MapHandler;
            Natives = lobbyDependencies.Natives;
            Notifications = lobbyDependencies.Notifications;
            Players = lobbyDependencies.Players;
            Teams = lobbyDependencies.Teams;
            Sounds = lobbyDependencies.Sounds;
            Sync = lobbyDependencies.Sync;
        }

        protected virtual async ValueTask CheckRemoveLobby()
        {
            if (!Entity.IsTemporary)
                return;

            if (await Players.Any())
                return;

            await Remove();
        }

        public virtual async Task Remove()
            => await Events.TriggerRemove();

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
