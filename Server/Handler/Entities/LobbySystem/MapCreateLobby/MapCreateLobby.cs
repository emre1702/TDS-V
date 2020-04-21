using BonusBotConnector.Client;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;
using TDS_Shared.Core;
using TDS_Server.Handler.Account;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class MapCreateLobby : Lobby
    {
        private int _lastId;
        private MapCreateDataDto _currentMap = new MapCreateDataDto();

        public MapCreateLobby(ITDSPlayer player, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler, 
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler) 
            : this(CreateEntity(player), dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, 
                  eventsHandler, bonusBotConnectorClient, bansHandler) 
            { 
            
            }

        public MapCreateLobby(Lobbies entity, TDSDbContext dbContext, ILoggingHandler loggingHandler, Serializer serializer, IModAPI modAPI, LobbiesHandler lobbiesHandler,
            ISettingsHandler settingsHandler, LangHelper langHelper, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler, 
            BonusBotConnectorClient bonusBotConnectorClient, BansHandler bansHandler)
            : base(entity, false, dbContext, loggingHandler, serializer, modAPI, lobbiesHandler, settingsHandler, langHelper, dataSyncHandler, eventsHandler, 
                  bonusBotConnectorClient, bansHandler)
        {

        }

        private static Lobbies CreateEntity(ITDSPlayer player)
        {
            Lobbies entity = new Lobbies
            {
                Name = "MapCreator-" + player.ModPlayer?.Name ?? "?",
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.ModPlayer?.Name ?? "?", ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = LobbyType.MapCreateLobby,
                OwnerId = player.Entity?.Id ?? -1,
                IsTemporary = true,
                DefaultSpawnX = -365.425f,
                DefaultSpawnY = -131.809f,
                DefaultSpawnZ = 37.873f,
                DefaultSpawnRotation = 0f
            };

            return entity;
        }

        public void StartNewMap()
        {
            _lastId = 0;
            _currentMap = new MapCreateDataDto();
            _posById = new Dictionary<int, MapCreatorPosition>();
            ModAPI.Sync.SendEvent(ToClientEvent.MapCreatorStartNewMap);
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
            ModAPI.Sync.SendEvent(this, ToClientEvent.LoadMapForMapCreator, json, _lastId);
        }
    }
}
