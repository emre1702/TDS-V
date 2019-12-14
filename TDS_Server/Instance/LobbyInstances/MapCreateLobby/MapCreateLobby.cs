using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.LobbyEntities;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class MapCreateLobby : Lobby
    {
        private int _lastId;
        private MapCreateDataDto _currentMap = new MapCreateDataDto();

        public MapCreateLobby(Lobbies entity) : base(entity) {}

        public static async void Create(TDSPlayer player)
        {
            if (player.Entity is null)
                return;

            Lobbies entity = new Lobbies
            {
                Name = "MapCreator-" + player.Client.Name,
                Teams = new List<Teams> { new Teams { Index = 0, Name = player.Client.Name, ColorR = 222, ColorB = 222, ColorG = 222 } },
                Type = ELobbyType.MapCreateLobby,
                OwnerId = player.Entity.Id,
                IsTemporary = true,
                DefaultSpawnX = -365.425f,
                DefaultSpawnY = -131.809f,
                DefaultSpawnZ = 37.873f,
                DefaultSpawnRotation = 0f
            };
            MapCreateLobby lobby = new MapCreateLobby(entity);
            await lobby.ExecuteForDBAsync(async (dbContext) => 
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();
            });

            await lobby.AddPlayer(player, 0);

            LobbyManager.AddLobby(lobby);
        }

        public void StartNewMap()
        {
            _lastId = 0;
            _currentMap = new MapCreateDataDto();
            _posById = new Dictionary<int, MapCreatorPosition>();
            SendAllPlayerEvent(DToClientEvent.MapCreatorStartNewMap, null);
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
            
            if (dto.MapCenter is { })
                _posById[dto.MapCenter.Id] = dto.MapCenter;
                
            if (dto.TeamSpawns is { })
                foreach (var list in dto.TeamSpawns)
                    foreach (var pos in list)
                        _posById[pos.Id] = pos;

            _lastId = _posById.Keys.Max();

            string json = Serializer.ToBrowser(dto);
            SendAllPlayerEvent(DToClientEvent.LoadMapForMapCreator, null, json);
        }
    }
}