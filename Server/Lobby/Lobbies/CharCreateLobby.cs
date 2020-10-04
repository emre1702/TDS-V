﻿using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.DependenciesModels;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.Players;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Lobbies
{
    public class CharCreateLobby : BaseLobby, ICharCreateLobby
    {
        public CharCreateLobby(LobbyDb entity, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
            : base(entity, databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider)
        {
        }

        public CharCreateLobby(ITDSPlayer player, DatabaseHandler databaseHandler, LangHelper langHelper, EventsHandler eventsHandler, LobbiesHandler lobbiesHandler,
            ILoggingHandler loggingHandler, IServiceProvider serviceProvider)
            : base(CreateEntity(player, lobbiesHandler.CharCreateLobbyDummy.Entity), databaseHandler, langHelper, eventsHandler, loggingHandler, serviceProvider)
        {
        }

        protected override void InitDependencies(BaseLobbyDependencies? lobbyDependencies = null)
        {
            lobbyDependencies ??= new CharCreateLobbyDependencies();

            lobbyDependencies.Bans ??= new CharCreateLobbyBansHandler(this, LangHelper);
            lobbyDependencies.Events ??= new BaseLobbyEventsHandler(this, GlobalEventsHandler, LoggingHandler);
            lobbyDependencies.Players ??= new CharCreateLobbyPlayers(this, lobbyDependencies.Events);

            base.InitDependencies(lobbyDependencies);
        }

        private static LobbyDb CreateEntity(ITDSPlayer player, LobbyDb dummy)
        {
            var entity = new LobbyDb
            {
                Name = "CharCreator-" + player.Name,
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Name, ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = LobbyType.CharCreateLobby,
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
