﻿using BonusBotConnector.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Entity.LobbySystem.BaseSystem;
using TDS_Server.Handler;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.LobbySystem.CharCreateLobbySystem
{
    public partial class CharCreateLobby : Lobby, ICharCreateLobby
    {
        #region Public Constructors

        public CharCreateLobby(ITDSPlayer player, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, IServiceProvider serviceProvider,
            IEntitiesByInterfaceCreator entitiesByInterfaceCreator)

            : this(CreateEntity(player, lobbiesHandler.CharCreateLobbyDummy.Entity), dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper,
                  eventsHandler, bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
        }

        public CharCreateLobby(Lobbies entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer,
            LobbiesHandler lobbiesHandler, ISettingsHandler settingsHandler, LangHelper langHelper, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, IServiceProvider serviceProvider, IEntitiesByInterfaceCreator entitiesByInterfaceCreator)

            : base(entity, false, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper,
                  eventsHandler, bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
        }

        #endregion Public Constructors

        #region Private Methods

        private static Lobbies CreateEntity(ITDSPlayer player, Lobbies dummy)
        {
            Lobbies entity = new Lobbies
            {
                Name = "CharCreator-" + player.Name ?? "?",
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Name ?? "?", ColorR = 222, ColorB = 222, ColorG = 222 } },
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

        #endregion Private Methods
    }
}
