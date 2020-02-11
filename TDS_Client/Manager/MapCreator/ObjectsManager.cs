using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.Lobby;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto.Map;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using Entity = RAGE.Elements.Entity;
using Ped = RAGE.Elements.Ped;
using Player = RAGE.Elements.Player;
using Vehicle = RAGE.Elements.Vehicle;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectsManager
    {
        private static readonly Dictionary<GameEntity, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<GameEntity, MapCreatorObject>();
        public static MapLimit MapLimitDisplay;

        public static void Start()
        {
            Events.OnEntityStreamIn += OnEntityStreamIn;
        }

        public static void Stop()
        {
            Events.OnEntityStreamIn -= OnEntityStreamIn;

            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Value.Delete(false);
            }
            _cacheMapEditorObjects.Clear();
            ObjectPlacing.CheckObjectDeleted();
            MapCreatorObject.Reset();

            MapLimitDisplay?.Stop();
            MapLimitDisplay = null;
        }

        public static MapCreatorObject GetByHandle(int handle)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Key.Handle == handle).Value;
        }

        public static MapCreatorObject GetByID(int id)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Value.ID == id).Value;
        }

        public static MapCreatorObject CreateMapCreatorObject(EMapCreatorPositionType type, object editingTeamIndexOrObjVehName, ushort playerRemoteId)
        {
            switch (type)
            {
                case EMapCreatorPositionType.TeamSpawn:
                    return GetTeamSpawn((int)editingTeamIndexOrObjVehName, playerRemoteId);
                case EMapCreatorPositionType.MapCenter:
                    return GetMapCenter(playerRemoteId);
                case EMapCreatorPositionType.BombPlantPlace:
                    return GetBombPlantPlace(playerRemoteId);
                case EMapCreatorPositionType.MapLimit:
                    return GetMapLimit(playerRemoteId);
                case EMapCreatorPositionType.Target:
                    return GetTarget(playerRemoteId);
                case EMapCreatorPositionType.Object:
                    string objName = (string)editingTeamIndexOrObjVehName;
                    uint objectHash = Misc.GetHashKey(objName);
                    return GetObject(objectHash, EMapCreatorPositionType.Object, playerRemoteId, objName);
                case EMapCreatorPositionType.Vehicle:
                    string vehName = (string)editingTeamIndexOrObjVehName;
                    // Todo: Check if this works or we need to use the VehicleHash enum instead
                    uint vehHash = Misc.GetHashKey(vehName);
                    return GetObject(vehHash, EMapCreatorPositionType.Vehicle, playerRemoteId, vehName);
            }
            return null;
        }

        public static MapCreatorObject GetOrCreateByHandle(int handle, EMapCreatorPositionType? type = null)
        {
            var alreadyCached = GetByHandle(handle);
            if (alreadyCached != null)
                return alreadyCached;
            var entityType = (EEntityType) RAGE.Game.Entity.GetEntityType(handle);

            GameEntityBase entity;
            switch (entityType)
            {
                case EEntityType.Ped:
                    entity = Entities.Peds.GetAtHandle(handle);
                    break;
                case EEntityType.Vehicle:
                    entity = Entities.Vehicles.GetAtHandle(handle);
                    break;
                case EEntityType.Object:
                    entity = Entities.Objects.GetAtHandle(handle);
                    break;
                default:
                   return null;
            }

            if (!type.HasValue)
            {
                switch (RAGE.Game.Entity.GetEntityType(handle))
                {
                    case (int)EEntityType.Ped:
                        type = EMapCreatorPositionType.TeamSpawn;
                        break;
                    case (int)EEntityType.Object:
                        type = EMapCreatorPositionType.Object;
                        break;
                    case (int)EEntityType.Vehicle:
                        type = EMapCreatorPositionType.Vehicle;
                        break;
                    default:
                        return null;
                }
            }

            var obj = new MapCreatorObject(entity, type.Value, Player.LocalPlayer.RemoteId);
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public static MapCreatorObject GetTeamSpawn(int editingTeamIndex, ushort playerRemoteId, int id = -1)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= ClientConstants.TeamSpawnPedHash.Length) 
                pedHashIndex -= ClientConstants.TeamSpawnPedHash.Length;
            var obj = new Ped(ClientConstants.TeamSpawnPedHash[pedHashIndex], Player.LocalPlayer.Position, Player.LocalPlayer.GetHeading(), dimension: Player.LocalPlayer.Dimension);
            obj.SetInvincible(true);
            var mapCreatorObj = new MapCreatorObject(obj, EMapCreatorPositionType.TeamSpawn, playerRemoteId, editingTeamIndex, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static MapCreatorObject GetMapCenter(ushort playerRemoteId, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == EMapCreatorPositionType.MapCenter);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(ClientConstants.MapCenterHash, EMapCreatorPositionType.MapCenter, playerRemoteId, id: id);
        }

        public static MapCreatorObject GetMapLimit(ushort playerRemoteId, int id = -1)
        {
            return GetObject(ClientConstants.MapLimitHash, EMapCreatorPositionType.MapLimit, playerRemoteId, id: id);
        }

        public static MapCreatorObject GetBombPlantPlace(ushort playerRemoteId, int id = -1)
        {
            return GetObject(ClientConstants.BombPlantPlaceHash, EMapCreatorPositionType.BombPlantPlace, playerRemoteId, id: id);
        }

        public static MapCreatorObject GetTarget(ushort playerRemoteId, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == EMapCreatorPositionType.Target);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(ClientConstants.TargetHash, EMapCreatorPositionType.Target, playerRemoteId, id: id);
        }

        public static MapCreatorObject GetVehicle(uint hash, ushort playerRemoteId, string vehName = null, int id = -1)
        {
            var vehicle = new Vehicle(hash, Player.LocalPlayer.Position, Player.LocalPlayer.GetHeading(), "Map", locked: true, dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(vehicle, EMapCreatorPositionType.Vehicle, playerRemoteId, objectName: vehName, id: id);
            _cacheMapEditorObjects[vehicle] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static MapCreatorObject GetObject(uint hash, EMapCreatorPositionType type, ushort playerRemoteId, string objName = null, int id = -1)
        {
            MapObject obj = new MapObject(hash, Player.LocalPlayer.Position, Player.LocalPlayer.GetRotation(2), dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(obj, type, playerRemoteId, objectName: objName, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static void DeleteTeamObjects(int teamNumber)
        {
            var teamObjects = _cacheMapEditorObjects.Where(entry => entry.Value.TeamNumber == teamNumber && CanEditObject(entry.Value)).ToList();
            foreach (var entry in teamObjects)
            {
                Delete(entry.Value);
            }
            ObjectPlacing.CheckObjectDeleted();
        }

        public static void Delete(int posId)
        {
            var obj = _cacheMapEditorObjects.FirstOrDefault(entry => entry.Value.ID == posId).Value;
            if (obj == null)
                return;
            Delete(obj);
        }

        public static void Delete(MapCreatorObject obj)
        {
            if (!CanEditObject(obj))
                return;
            _cacheMapEditorObjects.Remove(obj.Entity);
            obj.Delete(true);
            ObjectPlacing.CheckObjectDeleted();
            if (obj.Type == EMapCreatorPositionType.MapLimit)
            {
                RefreshMapLimitDisplay();
            }
        }

        public static void LoadMap(MapCreateDataDto map)
        {
            Stop();

            if (map.MapCenter != null)
            {
                var obj = GetMapCenter(map.MapCenter.OwnerRemoteId, map.MapCenter.Id);
                obj.LoadPos(map.MapCenter);
                if (TDSCamera.ActiveCamera != null)
                    TDSCamera.ActiveCamera.Position = new Vector3(map.MapCenter.PosX, map.MapCenter.PosY, map.MapCenter.PosZ);
                else
                    Player.LocalPlayer.Position = new Vector3(map.MapCenter.PosX, map.MapCenter.PosY, map.MapCenter.PosZ);
            }

            if (map.Target != null)
            {
                var obj = GetTarget(map.Target.OwnerRemoteId, map.Target.Id);
                obj.LoadPos(map.Target);
                if (TDSCamera.ActiveCamera != null)
                    TDSCamera.ActiveCamera.Position = new Vector3(map.Target.PosX, map.Target.PosY, map.Target.PosZ);
                else
                    Player.LocalPlayer.Position = new Vector3(map.Target.PosX, map.Target.PosY, map.Target.PosZ);
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
                    uint objectHash = Misc.GetHashKey(objName);
                    var obj = GetObject(objectHash, EMapCreatorPositionType.Object, objPos.OwnerRemoteId, objName, objPos.Id);
                    obj.LoadPos(objPos);
                }
            }

            if (map.Vehicles != null)
            {
                foreach (var vehPos in map.Vehicles)
                {
                    string vehName = Convert.ToString(vehPos.Info);
                    uint vehHash = Misc.GetHashKey(vehName);
                    var obj = GetVehicle(vehHash, vehPos.OwnerRemoteId, vehName, vehPos.Id);
                    obj.Freeze(true);
                    obj.LoadPos(vehPos);
                }
            }

            if (map.TeamSpawns != null)
            {

                foreach (var teamSpawns in map.TeamSpawns)
                {
                    foreach (var spawnPos in teamSpawns)
                    {
                        var obj = GetTeamSpawn(Convert.ToInt32(spawnPos.Info), spawnPos.OwnerRemoteId, spawnPos.Id);
                        obj.Freeze(true);
                        new TDSTimer(() =>
                        {
                            obj.Freeze(true);
                            obj.LoadPos(spawnPos);
                        }, 1000);
                     }
                }
            }

            Start();
        }

        public static void RefreshMapLimitDisplay()
        {
            MapLimitDisplay?.SetEdges(_cacheMapEditorObjects
                .Where(o => o.Value.Type == EMapCreatorPositionType.MapLimit)
                .Select(o => new Position3DDto { X = o.Value.MovingPosition.X, Y = o.Value.MovingPosition.Y, Z = o.Value.MovingPosition.Z })
                .ToList());
        }

        public static bool CanEditObject(MapCreatorObject obj)
        {
            return obj.OwnerRemoteId == Player.LocalPlayer.RemoteId || Lobby.Lobby.IsLobbyOwner;
        }

        public static IEnumerable<MapCreatorObject> GetAll()
        {
            return _cacheMapEditorObjects.Values;
        }


        private static void OnEntityStreamIn(Entity entity)
        {
            GameEntity ent = (GameEntity)entity;
            if (ent == null)
                return;

            var obj = GetByHandle(ent.Handle);
            if (obj == null)
                return;
            if (obj.Rotation is null)
                return;

            obj.MovingRotation = obj.Rotation;
        }
    }
}
