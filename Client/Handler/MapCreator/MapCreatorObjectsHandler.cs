using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Extensions;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Entities.GTA;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObjectsHandler : ServiceBase
    {
        public int IdCounter = 0;
        public MapLimit MapLimitDisplay;

        private readonly BrowserHandler _browserHandler;
        private readonly Dictionary<GameEntityBase, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<GameEntityBase, MapCreatorObject>();
        private readonly CamerasHandler _camerasHandler;
        private readonly EventsHandler _eventsHandler;

        //private readonly EventMethodData<EntityStreamOutDelegate> _entityStreamOutEventMethod;
        private readonly LobbyHandler _lobbyHandler;

        public MapCreatorObjectsHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, LobbyHandler lobbyHandler,
            EventsHandler eventsHandler, BrowserHandler browserHandler)
            : base(loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _lobbyHandler = lobbyHandler;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;

            //_entityStreamOutEventMethod = new EventMethodData<EntityStreamOutDelegate>(OnEntityStreamOut);

            eventsHandler.MapBorderColorChanged += EventsHandler_MapBorderColorChanged;

            RAGE.Events.Add(FromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            RAGE.Events.Add(FromBrowserEvent.RemoveMapCreatorTeamNumber, OnRemoveMapCreatorTeamNumberMethod);
            RAGE.Events.Add(ToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
        }

        public bool CanEditObject(MapCreatorObject obj)
        {
            return obj.OwnerRemoteId == Player.LocalPlayer.RemoteId || _lobbyHandler.IsLobbyOwner;
        }

        public MapCreatorObject CreateMapCreatorObject(MapCreatorPositionType type, object info, ushort ownerRemoteId,
            Vector3 pos, Vector3 rot)
        {
            switch (type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return GetTeamSpawn(Convert.ToInt32(info), ownerRemoteId, pos, rot);

                case MapCreatorPositionType.MapCenter:
                    return GetMapCenter(ownerRemoteId, pos, rot);

                case MapCreatorPositionType.BombPlantPlace:
                    return GetBombPlantPlace(ownerRemoteId, pos, rot);

                case MapCreatorPositionType.MapLimit:
                    return GetMapLimit(ownerRemoteId, pos, rot);

                case MapCreatorPositionType.Target:
                    return GetTarget(ownerRemoteId, pos, rot);

                case MapCreatorPositionType.Object:
                    string objName = (string)info;
                    return GetObject(objName, MapCreatorPositionType.Object, ownerRemoteId, pos, rot, objName);

                case MapCreatorPositionType.Vehicle:
                    string vehName = (string)info;
                    return GetVehicle(vehName, ownerRemoteId, vehName: vehName, pos: pos, rot: rot);
            }
            return null;
        }

        public MapCreatorObject CreateMapCreatorObject(MapCreatorPosition data)
        {
            switch (data.Type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    return GetTeamSpawn(Convert.ToInt32(data.Info), data.OwnerRemoteId,
                        new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), id: data.Id);

                case MapCreatorPositionType.MapCenter:
                    return GetMapCenter(data.OwnerRemoteId, new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), id: data.Id);

                case MapCreatorPositionType.BombPlantPlace:
                    return GetBombPlantPlace(data.OwnerRemoteId, new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), id: data.Id);

                case MapCreatorPositionType.MapLimit:
                    return GetMapLimit(data.OwnerRemoteId, new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), id: data.Id);

                case MapCreatorPositionType.Target:
                    return GetTarget(data.OwnerRemoteId, new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), id: data.Id);

                case MapCreatorPositionType.Object:
                    string objName = (string)data.Info;
                    return GetObject(objName, MapCreatorPositionType.Object, data.OwnerRemoteId,
                        new Vector3().GetPosFrom(data), new Vector3().GetRotFrom(data), objName, id: data.Id);

                case MapCreatorPositionType.Vehicle:
                    string vehName = (string)data.Info;
                    return GetVehicle(vehName, data.OwnerRemoteId, vehName: vehName, id: data.Id,
                        pos: new Vector3().GetPosFrom(data), rot: new Vector3().GetRotFrom(data));
            }
            return null;
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
            if (obj.Type == MapCreatorPositionType.MapLimit)
            {
                RefreshMapLimitDisplay();
            }
        }

        public void DeleteTeamObjects(int teamNumber, bool syncToServer = true)
        {
            var teamObjects = _cacheMapEditorObjects.Where(entry => entry.Value.TeamNumber == teamNumber && (!syncToServer || CanEditObject(entry.Value))).ToList();
            foreach (var entry in teamObjects)
            {
                Delete(entry.Value, false);
            }
            if (syncToServer)
                _eventsHandler.OnMapCreatorSyncTeamObjectsDeleted(teamNumber);
        }

        public MapCreatorObject FromDto(MapCreatorPosition dto)
        {
            var obj = CreateMapCreatorObject(dto);

            obj.LoadPos(dto);

            return obj;
        }

        public IEnumerable<MapCreatorObject> GetAll()
        {
            return _cacheMapEditorObjects.Values;
        }

        public MapCreatorObject GetBombPlantPlace(ushort playerRemoteId, Vector3 pos, Vector3 rot, int id = -1)
        {
            return GetObject(Constants.BombPlantPlaceHashName, MapCreatorPositionType.BombPlantPlace, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetByHandle(int handle)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Key.Handle == handle).Value;
        }

        public MapCreatorObject GetByID(int id)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Value.ID == id).Value;
        }

        public MapCreatorObject GetMapCenter(ushort playerRemoteId, Vector3 pos, Vector3 rot, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.MapCenter);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.MapCenterHashName, MapCreatorPositionType.MapCenter, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetMapLimit(ushort playerRemoteId, Vector3 pos, Vector3 rot, int id = -1)
        {
            return GetObject(Constants.MapLimitHashName, MapCreatorPositionType.MapLimit, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetObject(string hashName, MapCreatorPositionType type, ushort playerRemoteId,
            Vector3 pos, Vector3 rot, string objName = null, int id = -1)
        {
            uint hash = RAGE.Game.Misc.GetHashKey(hashName);
            var obj = new TDSObject(hash, pos, rot, dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(this, _eventsHandler, obj, type, playerRemoteId, pos, rot, objectName: objName, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetOrCreateByHandle(int handle, MapCreatorPositionType? type = null)
        {
            var alreadyCached = GetByHandle(handle);
            if (alreadyCached != null)
                return alreadyCached;
            var entityType = (EntityTypeInGetEntityType)RAGE.Game.Entity.GetEntityType(handle);

            GameEntityBase entity;
            switch (entityType)
            {
                case EntityTypeInGetEntityType.Ped:
                    entity = RAGE.Elements.Entities.Peds.GetAtHandle(handle);
                    break;

                case EntityTypeInGetEntityType.Vehicle:
                    entity = RAGE.Elements.Entities.Vehicles.GetAtHandle(handle);
                    break;

                case EntityTypeInGetEntityType.Object:
                    entity = RAGE.Elements.Entities.Objects.GetAtHandle(handle);
                    break;

                default:
                    return null;
            }

            if (!type.HasValue)
            {
                switch ((EntityTypeInGetEntityType)RAGE.Game.Entity.GetEntityType(handle))
                {
                    case EntityTypeInGetEntityType.Ped:
                        type = MapCreatorPositionType.TeamSpawn;
                        break;

                    case EntityTypeInGetEntityType.Object:
                        type = MapCreatorPositionType.Object;
                        break;

                    case EntityTypeInGetEntityType.Vehicle:
                        type = MapCreatorPositionType.Vehicle;
                        break;

                    default:
                        return null;
                }
            }

            var obj = new MapCreatorObject(this, _eventsHandler, entity, type.Value, RAGE.Elements.Player.LocalPlayer.RemoteId, entity.Position, entity.GetRotation(2));
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public MapCreatorObject GetTarget(ushort playerRemoteId, Vector3 pos, Vector3 rot, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.Target);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.TargetHashName, MapCreatorPositionType.Target, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetTeamSpawn(int editingTeamIndex, ushort playerRemoteId, Vector3 pos, Vector3 rot, int id = -1)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= Constants.TeamSpawnPedHash.Length)
                pedHashIndex -= Constants.TeamSpawnPedHash.Length;
            var obj = new TDSPed((uint)Constants.TeamSpawnPedHash[pedHashIndex], pos, rot.Z, dimension: Player.LocalPlayer.Dimension);
            obj.SetInvincible(true);
            obj.FreezePosition(true);
            var mapCreatorObj = new MapCreatorObject(this, _eventsHandler, obj, MapCreatorPositionType.TeamSpawn, playerRemoteId, pos, rot, editingTeamIndex, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetVehicle(string hashName, ushort playerRemoteId, Vector3 pos, Vector3 rot, string vehName = null, int id = -1)
        {
            uint hash = RAGE.Game.Misc.GetHashKey(hashName);
            var vehicle = new TDSVehicle(hash, pos, rot.Z, "Map",
                locked: true, dimension: Player.LocalPlayer.Dimension);
            vehicle.FreezePosition(true);
            //vehicle.SetCollision(false, true);
            var mapCreatorObj = new MapCreatorObject(this, _eventsHandler, vehicle, MapCreatorPositionType.Vehicle, playerRemoteId, pos, rot, objectName: vehName, id: id);
            _cacheMapEditorObjects[vehicle] = mapCreatorObj;
            return mapCreatorObj;
        }

        public void LoadMap(MapCreateDataDto map, int lastId)
        {
            Stop();
            Start();

            IdCounter = lastId;

            if (map.MapCenter != null)
            {
                var obj = GetMapCenter(map.MapCenter.OwnerRemoteId, new Vector3().GetPosFrom(map.MapCenter), new Vector3().GetRotFrom(map.MapCenter), map.MapCenter.Id);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = new Vector3().GetPosFrom(map.MapCenter);
                else
                    RAGE.Elements.Player.LocalPlayer.Position = new Vector3().GetPosFrom(map.MapCenter);
            }

            if (map.Target != null)
            {
                var obj = GetTarget(map.Target.OwnerRemoteId, new Vector3().GetPosFrom(map.Target), new Vector3().GetRotFrom(map.Target), map.Target.Id);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = new Vector3().GetPosFrom(map.Target);
                else
                    RAGE.Elements.Player.LocalPlayer.Position = new Vector3().GetPosFrom(map.Target);
            }

            if (map.BombPlaces != null)
            {
                foreach (var bombPlace in map.BombPlaces)
                {
                    var obj = GetBombPlantPlace(bombPlace.OwnerRemoteId, new Vector3().GetPosFrom(bombPlace), new Vector3().GetRotFrom(bombPlace), bombPlace.Id);
                }
            }

            if (map.MapEdges != null)
            {
                foreach (var mapEdge in map.MapEdges)
                {
                    var obj = GetMapLimit(mapEdge.OwnerRemoteId, new Vector3().GetPosFrom(mapEdge), new Vector3().GetRotFrom(mapEdge), mapEdge.Id);
                }
            }

            if (map.Objects != null)
            {
                foreach (var objPos in map.Objects)
                {
                    string objName = Convert.ToString(objPos.Info);
                    var obj = GetObject(objName, MapCreatorPositionType.Object, objPos.OwnerRemoteId,
                        new Vector3().GetPosFrom(objPos), new Vector3().GetRotFrom(objPos), objName, objPos.Id);
                }
            }

            if (map.Vehicles != null)
            {
                foreach (var vehPos in map.Vehicles)
                {
                    string vehName = Convert.ToString(vehPos.Info);
                    GetVehicle(vehName, vehPos.OwnerRemoteId, new Vector3().GetPosFrom(vehPos), new Vector3().GetRotFrom(vehPos), vehName, vehPos.Id);
                }
            }

            if (map.TeamSpawns != null)
            {
                foreach (var teamSpawns in map.TeamSpawns)
                {
                    foreach (var spawnPos in teamSpawns)
                    {
                        var obj = GetTeamSpawn(Convert.ToInt32(spawnPos.Info), spawnPos.OwnerRemoteId,
                            new Vector3().GetPosFrom(spawnPos), new Vector3().GetRotFrom(spawnPos), spawnPos.Id);
                    }
                }
            }

            foreach (var obj in GetAll())
            {
                obj.IsSynced = true;
            }

            //new TDSTimer(() =>
            //{
            foreach (var obj in GetAll())
            {
                obj.MovingRotation = obj.Rotation.Copy();
                obj.Freeze(true);
                //obj.SetCollision(false, true);
            }
            //}, 1000);
        }

        public void RefreshMapLimitDisplay()
        {
            MapLimitDisplay?.SetEdges(_cacheMapEditorObjects
                .Where(o => o.Value.Type == MapCreatorPositionType.MapLimit)
                .Select(o => o.Value.MovingPosition)
                .ToList());
        }

        public void Start()
        {
            RAGE.Events.OnEntityStreamIn += OnEntityStreamIn;
            //RAGE.Game.Event.EntityStreamOut.Add(_entityStreamOutEventMethod);
        }

        public void Stop()
        {
            RAGE.Events.OnEntityStreamIn -= OnEntityStreamIn;
            //RAGE.Game.Event.EntityStreamOut.Remove(_entityStreamOutEventMethod);

            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Value.Delete(false);
            }
            _cacheMapEditorObjects.Clear();
            IdCounter = 0;

            MapLimitDisplay?.Stop();
            MapLimitDisplay = null;
        }

        private void EventsHandler_MapBorderColorChanged(Color color)
        {
            if (!(MapLimitDisplay is null))
                MapLimitDisplay.MapBorderColor = color;
        }

        private void OnEntityStreamIn(Entity entity)
        {
            if (!(entity is ITDSObject mapObject))
                return;
            var obj = GetByHandle(mapObject.Handle);
            if (obj is null)
                return;
            if (obj.Rotation is null)
                return;

            obj.MovingRotation = obj.Rotation.Copy();
            obj.Freeze(true);
            //obj.SetCollision(false, true);
        }

        /*private void OnEntityStreamOut(IEntity entity)
        {
            Logging.LogWarning("Entity stream out: " + entity.Handle);
        }*/

        private void OnLoadMapForMapCreatorServerMethod(object[] args)
        {
            string json = (string)args[0];
            _browserHandler.Angular.LoadMapForMapCreator(json);

            int lastId = (int)args[1];
            var mapCreatorData = Serializer.FromServer<MapCreateDataDto>(json);
            LoadMap(mapCreatorData, lastId);
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
    }
}