using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Sync;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sync;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class MapCreatorLobby : FreeroamLobby, IMapCreatorLobby
    {
        public new IMapCreatorLobbySync Sync => (IMapCreatorLobbySync)base.Sync;

        public MapCreatorLobby(LobbyDb entity, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler)
            : base(entity, databaseHandler, langHelper, eventsHandler, serviceProvider, teamsProvider, loggingHandler)
        {
        }

        public MapCreatorLobby(ITDSPlayer player, IDatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            LobbiesHandler lobbiesHandler, IServiceProvider serviceProvider, ITeamsProvider teamsProvider, ILoggingHandler loggingHandler)
            : base(CreateEntity(player, lobbiesHandler.MapCreateLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler,
                  serviceProvider, teamsProvider, loggingHandler)
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
