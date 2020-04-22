using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObjectsHandler
    {
        private readonly Dictionary<IEntityBase, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<IEntityBase, MapCreatorObject>();
        public MapLimit MapLimitDisplay;
        public int IdCounter = 0;

        private readonly EventMethodData<EntityStreamInDelegate> _entityStreamInEventMethod;

        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly Serializer _serializer;

        public MapCreatorObjectsHandler(IModAPI modAPI, CamerasHandler camerasHandler, LobbyHandler lobbyHandler, EventsHandler eventsHandler, BrowserHandler browserHandler, 
            Serializer serializer)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _lobbyHandler = lobbyHandler;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;
            _serializer = serializer;

            _entityStreamInEventMethod = new EventMethodData<EntityStreamInDelegate>(OnEntityStreamIn);

            modAPI.Event.Add(FromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            modAPI.Event.Add(FromBrowserEvent.RemoveMapCreatorTeamNumber, OnRemoveMapCreatorTeamNumberMethod);
            modAPI.Event.Add(ToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
        }

        public void Start()
        {
            _modAPI.Event.EntityStreamIn.Add(_entityStreamInEventMethod);
        }

        public void Stop()
        {
            _modAPI.Event.EntityStreamIn.Remove(_entityStreamInEventMethod);

            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Value.Delete(false);
            }
            _cacheMapEditorObjects.Clear();
            _eventsHandler.OnMapCreatorObjectDeleted();
            IdCounter = 0;

            MapLimitDisplay?.Stop();
            MapLimitDisplay = null;
        }

        public MapCreatorObject GetByHandle(int handle)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Key.Handle == handle).Value;
        }

        public MapCreatorObject GetByID(int id)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Value.ID == id).Value;
        }

        public MapCreatorObject CreateMapCreatorObject(MapCreatorPositionType type, object editingTeamIndexOrObjVehName, ushort playerRemoteId)
        {
            switch (type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return GetTeamSpawn(Convert.ToInt32(editingTeamIndexOrObjVehName), playerRemoteId, _modAPI.LocalPlayer.Position, _modAPI.LocalPlayer.Rotation);
                case MapCreatorPositionType.MapCenter:
                    return GetMapCenter(playerRemoteId);
                case MapCreatorPositionType.BombPlantPlace:
                    return GetBombPlantPlace(playerRemoteId);
                case MapCreatorPositionType.MapLimit:
                    return GetMapLimit(playerRemoteId);
                case MapCreatorPositionType.Target:
                    return GetTarget(playerRemoteId);
                case MapCreatorPositionType.Object:
                    string objName = (string)editingTeamIndexOrObjVehName;
                    return GetObject(objName, MapCreatorPositionType.Object, playerRemoteId, objName);
                case MapCreatorPositionType.Vehicle:
                    string vehName = (string)editingTeamIndexOrObjVehName;
                    return GetVehicle(vehName, playerRemoteId, null, vehName: vehName);
            }
            return null;
        }

        public MapCreatorObject GetOrCreateByHandle(int handle, MapCreatorPositionType? type = null)
        {
            var alreadyCached = GetByHandle(handle);
            if (alreadyCached != null)
                return alreadyCached;
            var entityType = _modAPI.Entity.GetEntityType(handle);

            IEntityBase entity;
            switch (entityType)
            {
                case EntityType.Ped:
                    entity = _modAPI.Pool.Peds.GetAtHandle(handle);
                    break;
                case EntityType.Vehicle:
                    entity = _modAPI.Pool.Vehicles.GetAtHandle(handle);
                    break;
                case EntityType.Object:
                    entity = _modAPI.Pool.Objects.GetAtHandle(handle);
                    break;
                default:
                    return null;
            }

            if (!type.HasValue)
            {
                switch (_modAPI.Entity.GetEntityType(handle))
                {
                    case EntityType.Ped:
                        type = MapCreatorPositionType.TeamSpawn;
                        break;
                    case EntityType.Object:
                        type = MapCreatorPositionType.Object;
                        break;
                    case EntityType.Vehicle:
                        type = MapCreatorPositionType.Vehicle;
                        break;
                    default:
                        return null;
                }
            }

            var obj = new MapCreatorObject(_modAPI, this, _eventsHandler, entity, type.Value, _modAPI.LocalPlayer.RemoteId);
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public MapCreatorObject GetTeamSpawn(int editingTeamIndex, ushort playerRemoteId, Position3D pos, Position3D rot, int id = -1)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= Constants.TeamSpawnPedHash.Length)
                pedHashIndex -= Constants.TeamSpawnPedHash.Length;
            var obj = _modAPI.Ped.Create(Constants.TeamSpawnPedHash[pedHashIndex], pos, rot, dimension: _modAPI.LocalPlayer.Dimension);
            obj.SetInvincible(true);
            var mapCreatorObj = new MapCreatorObject(_modAPI, this, _eventsHandler, obj, MapCreatorPositionType.TeamSpawn, playerRemoteId, editingTeamIndex, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetMapCenter(ushort playerRemoteId, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.MapCenter);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.MapCenterHashName, MapCreatorPositionType.MapCenter, playerRemoteId, id: id);
        }

        public MapCreatorObject GetMapLimit(ushort playerRemoteId, int id = -1)
        {
            return GetObject(Constants.MapLimitHashName, MapCreatorPositionType.MapLimit, playerRemoteId, id: id);
        }

        public MapCreatorObject GetBombPlantPlace(ushort playerRemoteId, int id = -1)
        {
            return GetObject(Constants.BombPlantPlaceHashName, MapCreatorPositionType.BombPlantPlace, playerRemoteId, id: id);
        }

        public MapCreatorObject GetTarget(ushort playerRemoteId, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.Target);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.TargetHashName, MapCreatorPositionType.Target, playerRemoteId, id: id);
        }

        public MapCreatorObject GetVehicle(string hashName, ushort playerRemoteId, Position3D pos = null, Position3D rot = null, string vehName = null, int id = -1)
        {
            uint hash = _modAPI.Misc.GetHashKey(hashName);
            var vehicle = _modAPI.Vehicle.Create(hash, pos ?? _modAPI.LocalPlayer.Position, rot ?? _modAPI.LocalPlayer.Rotation, "Map", 
                locked: true, dimension: _modAPI.LocalPlayer.Dimension);
            vehicle.FreezePosition(true);
            vehicle.SetCollision(false, true);
            var mapCreatorObj = new MapCreatorObject(_modAPI, this, _eventsHandler, vehicle, MapCreatorPositionType.Vehicle, playerRemoteId, objectName: vehName, id: id);
            _cacheMapEditorObjects[vehicle] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetObject(string hashName, MapCreatorPositionType type, ushort playerRemoteId, string objName = null, int id = -1)
        {
            uint hash = _modAPI.Misc.GetHashKey(hashName);
            var obj = _modAPI.MapObject.Create(hash, _modAPI.LocalPlayer.Position, _modAPI.LocalPlayer.Rotation, dimension: _modAPI.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(_modAPI, this, _eventsHandler, obj, type, playerRemoteId, objectName: objName, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public void DeleteTeamObjects(int teamNumber, bool syncToServer = true)
        {
            var teamObjects = _cacheMapEditorObjects.Where(entry => entry.Value.TeamNumber == teamNumber && (!syncToServer || CanEditObject(entry.Value))).ToList();
            foreach (var entry in teamObjects)
            {
                Delete(entry.Value, false);
            }
            _eventsHandler.OnMapCreatorObjectDeleted();
            if (syncToServer)
                _eventsHandler.OnMapCreatorSyncTeamObjectsDeleted(teamNumber);
        }

        public void Delete(int posId)
        {
            var obj = _cacheMapEditorObjects.FirstOrDefault(entry => entry.Value.ID == posId).Value;
            if (obj == null)
                return;
            Delete(obj);
        }

        public void Delete(MapCreatorObject obj, bool syncToServer = true)
        {
            if (!CanEditObject(obj) && syncToServer)
                return;
            _cacheMapEditorObjects.Remove(obj.Entity);
            obj.Delete(syncToServer);
            _eventsHandler.OnMapCreatorObjectDeleted();
            if (obj.Type == MapCreatorPositionType.MapLimit)
            {
                RefreshMapLimitDisplay();
            }
        }

        public MapCreatorObject FromDto(MapCreatorPosition dto)
        {
            var obj = CreateMapCreatorObject(dto.Type, dto.Info, dto.OwnerRemoteId);

            obj.LoadPos(dto);

            return obj;
        }

        public void LoadMap(MapCreateDataDto map, int lastId)
        {
            Stop();

            IdCounter = lastId;

            if (map.MapCenter != null)
            {
                var obj = GetMapCenter(map.MapCenter.OwnerRemoteId, map.MapCenter.Id);
                obj.LoadPos(map.MapCenter);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = new Position3D(map.MapCenter);
                else
                    _modAPI.LocalPlayer.Position = new Position3D(map.MapCenter);
            }

            if (map.Target != null)
            {
                var obj = GetTarget(map.Target.OwnerRemoteId, map.Target.Id);
                obj.LoadPos(map.Target);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = new Position3D(map.Target);
                else
                    _modAPI.LocalPlayer.Position = new Position3D(map.Target);
            }

            if (map.BombPlaces != null)
            {
                foreach (var bombPlace in map.BombPlaces)
                {
                    var obj = GetBombPlantPlace(bombPlace.OwnerRemoteId, bombPlace.Id);
                    obj.LoadPos(bombPlace);
                }
            }

            if (map.MapEdges != null)
            {
                foreach (var mapEdge in map.MapEdges)
                {
                    var obj = GetMapLimit(mapEdge.OwnerRemoteId, mapEdge.Id);
                    obj.LoadPos(mapEdge);
                }
            }

            if (map.Objects != null)
            {
                foreach (var objPos in map.Objects)
                {
                    string objName = Convert.ToString(objPos.Info);
                    var obj = GetObject(objName, MapCreatorPositionType.Object, objPos.OwnerRemoteId, objName, objPos.Id);
                    obj.LoadPos(objPos);
                }
            }

            if (map.Vehicles != null)
            {
                foreach (var vehPos in map.Vehicles)
                {
                    string vehName = Convert.ToString(vehPos.Info);
                    GetVehicle(vehName, vehPos.OwnerRemoteId, new Position3D(vehPos), 
                        new Position3D(vehPos.RotX, vehPos.RotY, vehPos.RotZ), vehName, vehPos.Id);
                    //obj.Freeze(true); already done in GetVehicle
                }
            }

            if (map.TeamSpawns != null)
            {

                foreach (var teamSpawns in map.TeamSpawns)
                {
                    foreach (var spawnPos in teamSpawns)
                    {
                        var obj = GetTeamSpawn(Convert.ToInt32(spawnPos.Info), spawnPos.OwnerRemoteId, new Position3D(spawnPos), 
                            new Position3D(spawnPos.RotX, spawnPos.RotY, spawnPos.RotZ), spawnPos.Id);
                        obj.Freeze(true);
                    }
                }
            }

            foreach (var obj in GetAll())
                obj.IsSynced = true;

            new TDSTimer(() =>
            {
                foreach (var obj in GetAll())
                {
                    obj.Freeze(true);
                    obj.SetCollision(false, true);
                }
                    
            }, 3000);

            Start();
        }

        public void RefreshMapLimitDisplay()
        {
            MapLimitDisplay?.SetEdges(_cacheMapEditorObjects
                .Where(o => o.Value.Type == MapCreatorPositionType.MapLimit)
                .Select(o => o.Value.MovingPosition)
                .ToList());
        }

        public bool CanEditObject(MapCreatorObject obj)
        {
            return obj.OwnerRemoteId == _modAPI.LocalPlayer.RemoteId || _lobbyHandler.IsLobbyOwner;
        }

        public IEnumerable<MapCreatorObject> GetAll()
        {
            return _cacheMapEditorObjects.Values;
        }


        private void OnEntityStreamIn(IEntity entity)
        {
            var obj = GetByHandle(entity.Handle);
            if (obj == null)
                return;
            if (obj.Rotation is null)
                return;

            obj.MovingRotation = new Position3D(obj.Rotation);
        }

        private void OnRemoveMapCreatorPositionMethod(object[] args)
        {
            int posId = Convert.ToInt32(args[0]);
            Delete(posId);
        }

        private void OnRemoveMapCreatorTeamNumberMethod(object[] args)
        {
            int teamNumber = Convert.ToInt32(args[0]);
            DeleteTeamObjects(teamNumber);
        }

        private void OnLoadMapForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            _browserHandler.Angular.LoadMapForMapCreator(json);

            int lastId = (int)args[1];
            var mapCreatorData = _serializer.FromServer<MapCreateDataDto>(json);
            LoadMap(mapCreatorData, lastId);
        }
    }
}
