﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class MapCreatorLobbySync : BaseLobbySync
    {
        private int _lastId;
        private MapCreateDataDto _currentMap = new MapCreateDataDto();
        private Dictionary<int, MapCreatorPosition> _posById = new Dictionary<int, MapCreatorPosition>();
        private readonly SemaphoreSlim _posDictSemaphore = new SemaphoreSlim(1, 1);

        protected new MapCreatorLobbyPlayers Players => (MapCreatorLobbyPlayers)base.Players;

        public MapCreatorLobbySync(LobbyDb entity, IBaseLobbyEventsHandler events, Func<uint> dimensionProvider, MapCreatorLobbyPlayers players, BaseLobbyTeamsHandler teams)
            : base(entity, events, dimensionProvider, players, teams)
        {
            events.PlayerJoined += Events_PlayerJoined;
        }

        public async void StartNewMap()
        {
            _currentMap = new MapCreateDataDto();
            _lastId = 0;
            await _posDictSemaphore.Do(() =>
            {
                _posById = new Dictionary<int, MapCreatorPosition>();
            });
            NAPI.Task.Run(()
                => TriggerEvent(ToClientEvent.MapCreatorStartNewMap));
        }

        public async Task SetMap(MapCreateDataDto dto)
        {
            _currentMap = dto;
            await DoForDictionary(dict =>
            {
                if (dto.BombPlaces is { })
                    foreach (var pos in dto.BombPlaces)
                        dict[pos.Id] = pos;

                if (dto.MapEdges is { })
                    foreach (var pos in dto.MapEdges)
                        dict[pos.Id] = pos;

                if (dto.Objects is { })
                    foreach (var pos in dto.Objects)
                        dict[pos.Id] = pos;

                if (dto.Vehicles is { })
                    foreach (var pos in dto.Vehicles)
                        dict[pos.Id] = pos;

                if (dto.MapCenter is { })
                    dict[dto.MapCenter.Id] = dto.MapCenter;

                if (dto.TeamSpawns is { })
                    foreach (var list in dto.TeamSpawns)
                        foreach (var pos in list)
                            dict[pos.Id] = pos;

                if (dto.Target is { })
                    dict[dto.Target.Id] = dto.Target;

                _lastId = dict.Keys.Max();
            });

            string json = Serializer.ToBrowser(dto);
            NAPI.Task.Run(()
                => TriggerEvent(ToClientEvent.LoadMapForMapCreator, json, _lastId));
        }

        internal Task DoForDictionary(Action<Dictionary<int, MapCreatorPosition>> action)
            => _posDictSemaphore.Do(() => action(_posById));

        internal async Task SetSyncedMapAndSyncToPlayer(string json, int tdsPlayerId, int lastId)
        {
            _currentMap = Serializer.FromBrowser<MapCreateDataDto>(json);
            _lastId = lastId;
            var player = await Players.GetById(tdsPlayerId);
            if (player is null)
                return;
            NAPI.Task.Run(() =>
                player.TriggerEvent(ToClientEvent.MapCreatorSyncAllObjects, json, lastId));
        }

        public void SyncLastId(ITDSPlayer player, int lastId)
        {
            if (_lastId >= lastId)
            {
                NAPI.Task.Run(() =>
                    player.TriggerEvent(ToClientEvent.MapCreatorSyncFixLastId, lastId, _lastId));
            }
            else
            {
                _lastId = lastId;
            }
        }

        public void SyncMapInfoChange(MapCreatorInfoType infoType, object data)
        {
            TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.MapCreatorSyncData, (int)infoType, data);

            switch (infoType)
            {
                case MapCreatorInfoType.DescriptionEnglish:
                    _currentMap.Description[(int)Language.English] = Convert.ToString(data);
                    break;

                case MapCreatorInfoType.DescriptionGerman:
                    _currentMap.Description[(int)Language.German] = Convert.ToString(data);
                    break;

                case MapCreatorInfoType.Name:
                    _currentMap.Name = Convert.ToString(data);
                    break;

                case MapCreatorInfoType.Type:
                    _currentMap.Type = (MapType)Convert.ToInt32(data);
                    break;

                case MapCreatorInfoType.Settings:
                    _currentMap.Settings = Serializer.FromBrowser<MapCreateSettings>(Convert.ToString(data));
                    break;
            }
        }

        public async void SyncNewObject(ITDSPlayer player, string json)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers((await Players.GetExcept(player)).ToArray(), ToClientEvent.MapCreatorSyncNewObject, json);

            var pos = Serializer.FromClient<MapCreatorPosition>(json);
            if (pos.Type == MapCreatorPositionType.MapCenter)
                _currentMap.MapCenter = pos;
            else if (pos.Type == MapCreatorPositionType.Target)
                _currentMap.Target = pos;
            else
                GetListInCurrentMapForMapType(pos.Type, pos.Info)?.Add(pos);
        }

        public async void SyncObjectPosition(ITDSPlayer player, string json)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers((await Players.GetExcept(player)).ToArray(),
                ToClientEvent.MapCreatorSyncObjectPosition, json);

            var pos = Serializer.FromClient<MapCreatorPosData>(json);

            var data = await GetPosById(pos.Id);
            if (data is null)
                return;

            data.PosX = pos.PosX;
            data.PosY = pos.PosY;
            data.PosZ = pos.PosZ;
            data.RotX = pos.RotX;
            data.RotY = pos.RotY;
            data.RotZ = pos.RotZ;
        }

        private Task<MapCreatorPosition?> GetPosById(int posId)
            => _posDictSemaphore.Do(() =>
            {
                _posById.TryGetValue(posId, out var value);
                return value;
            });

        public async void SyncRemoveObject(ITDSPlayer player, int objId)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers((await Players.GetExcept(player)).ToArray(), ToClientEvent.MapCreatorSyncObjectRemove, objId);

            if (!_posById.ContainsKey(objId))
                return;
            var data = _posById[objId];
            if (data.Type == MapCreatorPositionType.MapCenter)
                _currentMap.MapCenter = null;
            else if (data.Type == MapCreatorPositionType.Target)
                _currentMap.Target = null;
            GetListInCurrentMapForMapType(data.Type, data.Info)?.Remove(data);
            _posById.Remove(objId);
        }

        public async void SyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers((await Players.GetExcept(player)).ToArray(), ToClientEvent.MapCreatorSyncTeamObjectsRemove, teamNumber);

            foreach (var entry in _posById)
            {
                if (entry.Value.Type != MapCreatorPositionType.TeamSpawn)
                    continue;
                if ((int)entry.Value.Info != teamNumber)
                    continue;

                GetListInCurrentMapForMapType(entry.Value.Type, entry.Value.Info)?.Remove(entry.Value);
            }
        }

        private List<MapCreatorPosition>? GetListInCurrentMapForMapType(MapCreatorPositionType type, object? info)
        {
            switch (type)
            {
                case MapCreatorPositionType.BombPlantPlace:
                    if (_currentMap.BombPlaces is null)
                        _currentMap.BombPlaces = new List<MapCreatorPosition>();
                    return _currentMap.BombPlaces;

                case MapCreatorPositionType.MapLimit:
                    if (_currentMap.MapEdges is null)
                        _currentMap.MapEdges = new List<MapCreatorPosition>();
                    return _currentMap.MapEdges;

                case MapCreatorPositionType.Object:
                    if (_currentMap.Objects is null)
                        _currentMap.Objects = new List<MapCreatorPosition>();
                    return _currentMap.Objects;

                case MapCreatorPositionType.Vehicle:
                    if (_currentMap.Vehicles is null)
                        _currentMap.Vehicles = new List<MapCreatorPosition>();
                    return _currentMap.Vehicles;

                case MapCreatorPositionType.TeamSpawn:
                    if (_currentMap.TeamSpawns is null)
                        _currentMap.TeamSpawns = new List<List<MapCreatorPosition>>();
                    if (!int.TryParse(info?.ToString(), out int teamIndex))
                        return null;
                    while (_currentMap.TeamSpawns.Count <= teamIndex)
                        _currentMap.TeamSpawns.Add(new List<MapCreatorPosition>());
                    return _currentMap.TeamSpawns[teamIndex];
            }
            return null;
        }

        private async void Events_PlayerJoined(ITDSPlayer player, int teamIndex)
        {
            var firstPlayer = await Players.GetFirst(player);
            NAPI.Task.Run(() =>
            {
                if (Players.Count == 2)
                    firstPlayer?.TriggerEvent(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, player.Id);
                else if (Players.Count > 2)
                    player.TriggerEvent(ToClientEvent.MapCreatorSyncAllObjects, Serializer.ToBrowser(_currentMap), _lastId);
            });
        }
    }
}
