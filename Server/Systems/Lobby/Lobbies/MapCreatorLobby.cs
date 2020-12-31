using System;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Sync;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.DependenciesModels;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Server.LobbySystem.Lobbies.Abstracts;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.Sync;
using TDS.Shared.Data.Enums;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Lobbies
{
    public class MapCreatorLobby : FreeroamLobby, IMapCreatorLobby
    {
        public new IMapCreatorLobbySync Sync => (IMapCreatorLobbySync)base.Sync;

        public MapCreatorLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        public MapCreatorLobby(ITDSPlayer player, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            LobbiesHandler lobbiesHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(CreateEntity(player, lobbiesHandler.MapCreateLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler,
                  serviceProvider, teamsProvider, loggingHandler, lobbiesHandler, remoteBrowserEventsHandler)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new MapCreatorLobbyDependencies();

            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.Sync ??= new MapCreatorLobbySync(this, lobbyDependencies.Events);
            lobbyDependencies.Bans ??= new MapCreatorLobbyBansHandler(this, LangHelper);
            lobbyDependencies.MapHandler ??= new MapCreatorLobbyMapHandler(this, lobbyDependencies.Events);
            lobbyDependencies.Players ??= new MapCreatorLobbyPlayers(this, lobbyDependencies.Events);

            base.InitDependencies(lobbyDependencies);
        }

        private static LobbyDb CreateEntity(ITDSPlayer player, LobbyDb dummy)
        {
            var entity = new LobbyDb
            {
                Name = "MapCreator-" + player.Name ?? "?",
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Name ?? "?", ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = LobbyType.MapCreateLobby,
                OwnerId = player.Entity?.Id ?? -1,
                IsTemporary = true,
                DefaultSpawnX = dummy.DefaultSpawnX,
                DefaultSpawnY = dummy.DefaultSpawnY,
                DefaultSpawnZ = dummy.DefaultSpawnZ,
                DefaultSpawnRotation = dummy.DefaultSpawnRotation
            };

            return entity;
        }
    }
}