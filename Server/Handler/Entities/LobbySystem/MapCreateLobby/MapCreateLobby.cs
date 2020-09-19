using BonusBotConnector.Client;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class MapCreateLobby : Lobby
    {
        private MapCreateDataDto _currentMap = new MapCreateDataDto();
        private int _lastId;

        public MapCreateLobby(ITDSPlayer player, TDSDbContext dbContext, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : this(CreateEntity(player, lobbiesHandler.MapCreateLobbyDummy.Entity), dbContext, loggingHandler, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler,
                  eventsHandler, bonusBotConnectorClient, bansHandler)
        {
        }

        public MapCreateLobby(Lobbies entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler,
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : base(entity, false, dbContext, loggingHandler, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler,
                  bonusBotConnectorClient, bansHandler)
        {
        }

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
            TriggerEvent(ToClientEvent.LoadMapForMapCreator, json, _lastId);
        }

        public void StartNewMap()
        {
            _lastId = 0;
            _currentMap = new MapCreateDataDto();
            _posById = new Dictionary<int, MapCreatorPosition>();
            TriggerEvent(ToClientEvent.MapCreatorStartNewMap);
        }

        private static Lobbies CreateEntity(ITDSPlayer player, Lobbies dummy)
        {
            var entity = new Lobbies
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
