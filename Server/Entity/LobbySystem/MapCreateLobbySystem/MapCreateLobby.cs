﻿using BonusBotConnector.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.MapCreateLobbySystem
{
    public partial class MapCreateLobby : Lobby, IMapCreateLobby
    {
        #region Fields

        private MapCreateDataDto _currentMap = new MapCreateDataDto();
        private int _lastId;

        #endregion Fields

        #region Constructors

        public MapCreateLobby(ITDSPlayer player, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, IServiceProvider serviceProvider,
            IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            : this(CreateEntity(player, lobbiesHandler.MapCreateLobbyDummy.Entity), dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper,
                  eventsHandler, bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
        }

        public MapCreateLobby(Lobbies entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler, IServiceProvider serviceProvider, 
            IEntitiesByInterfaceCreator entitiesByInterfaceCreator)
            : base(entity, false, dbContext, loggingHandler, serializer, lobbiesHandler, settingsHandler, langHelper, eventsHandler,
                  bonusBotConnectorClient, bansHandler, serviceProvider, entitiesByInterfaceCreator)
        {
        }

        #endregion Constructors

        #region Methods

        public void SetMap(MapCreateDataDto dto)
        {
            _currentMap = dto;

            if (dto.BombPlaces is { })
                foreach (var pos in dto.BombPlaces)
                    _posById[pos.Id] = pos;

            if (dto.MapEdges is { })
                foreach (var pos in dto.MapEdges)
                    _posById[pos.Id] = pos;

            if (dto.Objects is { })
                foreach (var pos in dto.Objects)
                    _posById[pos.Id] = pos;

            if (dto.Vehicles is { })
                foreach (var pos in dto.Vehicles)
                    _posById[pos.Id] = pos;

            if (dto.MapCenter is { })
                _posById[dto.MapCenter.Id] = dto.MapCenter;

            if (dto.TeamSpawns is { })
                foreach (var list in dto.TeamSpawns)
                    foreach (var pos in list)
                        _posById[pos.Id] = pos;

            if (dto.Target is { })
                _posById[dto.Target.Id] = dto.Target;

            _lastId = _posById.Keys.Max();

            string json = Serializer.ToBrowser(dto);
            SendEvent(ToClientEvent.LoadMapForMapCreator, json, _lastId);
        }

        public void StartNewMap()
        {
            _lastId = 0;
            _currentMap = new MapCreateDataDto();
            _posById = new Dictionary<int, MapCreatorPosition>();
            SendEvent(ToClientEvent.MapCreatorStartNewMap);
        }

        private static Lobbies CreateEntity(ITDSPlayer player, Lobbies dummy)
        {
            Lobbies entity = new Lobbies
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

        #endregion Methods
    }
}