using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class MapCreateLobby
    {
        private Dictionary<int, MapCreatorPosition> _posById = new Dictionary<int, MapCreatorPosition>();

        public void SyncLastId(ITDSPlayer player, int lastId)
        {
            if (_lastId >= lastId)
            {
                player.SendEvent(ToClientEvent.MapCreatorSyncFixLastId, lastId, _lastId);
            }
            else
            {
                _lastId = lastId;
            }

        }

        public void SyncAllObjectsToPlayer(int tdsPlayerId, string json, int lastId)
        {
            _lastId = lastId;
            Players.TryGetValue(tdsPlayerId, out ITDSPlayer? player);
            if (player is null)
                return;
            player.SendEvent(ToClientEvent.MapCreatorSyncAllObjects, json, lastId);
        }

        public void SyncNewObject(ITDSPlayer player, string json)
        {
            ModAPI.Sync.SendEvent(Players.Values.Where(p => p != player).ToList(), ToClientEvent.MapCreatorSyncNewObject, json);

            var pos = Serializer.FromClient<MapCreatorPosition>(json);
            if (pos.Type == MapCreatorPositionType.MapCenter)
                _currentMap.MapCenter = pos;
            else if (pos.Type == MapCreatorPositionType.Target)
                _currentMap.Target = pos;
            else
                GetListInCurrentMapForMapType(pos.Type, pos.Info)?.Add(pos);
        }

        public void SyncObjectPosition(ITDSPlayer player, string json)
        {
            ModAPI.Sync.SendEvent(Players.Values.Where(p => p != player).ToList(),
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

        public void SyncRemoveObject(ITDSPlayer player, int objId)
        {
            ModAPI.Sync.SendEvent(Players.Values.Where(p => p != player), ToClientEvent.MapCreatorSyncObjectRemove, objId);

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

        public void SyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            ModAPI.Sync.SendEvent(Players.Values.Where(p => p != player), ToClientEvent.MapCreatorSyncTeamObjectsRemove, teamNumber);

            foreach (var entry in _posById)
            {
                if (entry.Value.Type != MapCreatorPositionType.TeamSpawn)
                    continue;
                if ((int)entry.Value.Info != teamNumber)
                    continue;

                GetListInCurrentMapForMapType(entry.Value.Type, entry.Value.Info)?.Remove(entry.Value);
            }
        }

        public void SyncMapInfoChange(MapCreatorInfoType infoType, object data)
        {
            ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.MapCreatorSyncData, (int)infoType, data);

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
    }
}
