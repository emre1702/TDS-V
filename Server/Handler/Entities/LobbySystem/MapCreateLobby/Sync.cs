﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto.Map.Creator;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class MapCreateLobby
    {
        private Dictionary<int, MapCreatorPosition> _posById = new Dictionary<int, MapCreatorPosition>();

        public void SyncLastId(TDSPlayer player, int lastId)
        {
            if (_lastId >= lastId)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.MapCreatorSyncFixLastId, lastId, _lastId);
            }
            else
            {
                _lastId = lastId;
            }
            
        }

        public void SyncAllObjectsToPlayer(int tdsPlayerId, string json)
        {
            var player = GetPlayerById(tdsPlayerId);
            if (player is null)
                return;
            NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.MapCreatorSyncAllObjects, json);
        }

        public void SyncNewObject(TDSPlayer player, string json)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p != player).Select(p => p.Player).ToArray(), 
                ToClientEvent.MapCreatorSyncNewObject, json);

            var pos = Serializer.FromClient<MapCreatorPosition>(json);
            if (pos.Type == EMapCreatorPositionType.MapCenter)
                _currentMap.MapCenter = pos;
            else if (pos.Type == EMapCreatorPositionType.Target)
                _currentMap.Target = pos;
            else
                GetListInCurrentMapForMapType(pos.Type, pos.Info)?.Add(pos);
        }

        public void SyncObjectPosition(TDSPlayer player, string json)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p != player).Select(p => p.Player).ToArray(), 
                ToClientEvent.MapCreatorSyncObjectPosition, json);

            var pos = Serializer.FromClient<MapCreatorPosData>(json);
            if (!_posById.ContainsKey(pos.Id))
                return;
            var data = _posById[pos.Id];

            data.PosX = pos.PosX;
            data.PosY = pos.PosY;
            data.PosZ = pos.PosZ;
            data.RotX = pos.RotX;
            data.RotY = pos.RotY;
            data.RotZ = pos.RotZ;
        }

        public void SyncRemoveObject(TDSPlayer player, int objId)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p != player).Select(p => p.Player).ToArray(),
                ToClientEvent.MapCreatorSyncObjectRemove, objId);

            if (!_posById.ContainsKey(objId))
                return;
            var data = _posById[objId];
            if (data.Type == EMapCreatorPositionType.MapCenter)
                _currentMap.MapCenter = null;
            else if (data.Type == EMapCreatorPositionType.Target)
                _currentMap.Target = null;
            GetListInCurrentMapForMapType(data.Type, data.Info)?.Remove(data);
        }

        public void SyncMapInfoChange(EMapCreatorInfoType infoType, object data)
        {
            SendAllPlayerEvent(ToClientEvent.MapCreatorSyncData, null, infoType, data);

            switch (infoType)
            {
                case EMapCreatorInfoType.DescriptionEnglish:
                    _currentMap.Description[(int)Language.English] = Convert.ToString(data);
                    break;
                case EMapCreatorInfoType.DescriptionGerman:
                    _currentMap.Description[(int)Language.German] = Convert.ToString(data);
                    break;
                case EMapCreatorInfoType.Name:
                    _currentMap.Name = Convert.ToString(data);
                    break;
                case EMapCreatorInfoType.Type:
                    _currentMap.Type = (EMapType)Convert.ToInt32(data);
                    break;
                case EMapCreatorInfoType.Settings:
                    _currentMap.Settings = Serializer.FromBrowser<MapCreateSettings>(Convert.ToString(data));
                    break;
            }

        }

        private List<MapCreatorPosition>? GetListInCurrentMapForMapType(EMapCreatorPositionType type, object? info)
        {
            switch (type)
            {
                case EMapCreatorPositionType.BombPlantPlace:
                    if (_currentMap.BombPlaces is null)
                        _currentMap.BombPlaces = new List<MapCreatorPosition>();
                    return _currentMap.BombPlaces;
                case EMapCreatorPositionType.MapLimit:
                    if (_currentMap.MapEdges is null)
                        _currentMap.MapEdges = new List<MapCreatorPosition>();
                    return _currentMap.MapEdges;
                case EMapCreatorPositionType.Object:
                    if (_currentMap.Objects is null)
                        _currentMap.Objects = new List<MapCreatorPosition>();
                    return _currentMap.Objects;
                case EMapCreatorPositionType.Vehicle:
                    if (_currentMap.Vehicles is null)
                        _currentMap.Vehicles = new List<MapCreatorPosition>();
                    return _currentMap.Vehicles;
                case EMapCreatorPositionType.TeamSpawn:
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
    }
}
