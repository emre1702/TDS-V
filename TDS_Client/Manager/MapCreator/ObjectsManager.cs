using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto.Map;
using TDS_Common.Dto.Map.Creator;
using TDS_Common.Enum;
using Entity = RAGE.Elements.Entity;
using Ped = RAGE.Elements.Ped;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectsManager
    {
        private static readonly Dictionary<GameEntity, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<GameEntity, MapCreatorObject>();

        public static void Start()
        {
            Events.OnEntityStreamIn += OnEntityStreamIn;
        }

        public static void Stop()
        {
            Events.OnEntityStreamIn -= OnEntityStreamIn;

            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Value.Delete();
            }
            _cacheMapEditorObjects.Clear();
            ObjectPlacing.CheckObjectDeleted();
        }

        public static void Add(GameEntityBase obj, EMapCreatorPositionType type)
        {
            var mapCreatorObject = new MapCreatorObject(obj, type);
            _cacheMapEditorObjects[obj] = mapCreatorObject;
        }

        public static MapCreatorObject GetByHandle(int handle)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Key.Handle == handle).Value;
        }

        public static MapCreatorObject GetByID(int id)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Value.ID == id).Value;
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
                    default:
                        return null;
                }
            }

            var obj = new MapCreatorObject(entity, type.Value);
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public static MapCreatorObject GetTeamSpawn(int editingTeamIndex, int id = -1)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= ClientConstants.TeamSpawnPedHash.Length) 
                pedHashIndex -= ClientConstants.TeamSpawnPedHash.Length;
            var obj = new Ped(ClientConstants.TeamSpawnPedHash[pedHashIndex], Player.LocalPlayer.Position, Player.LocalPlayer.GetHeading(), dimension: Player.LocalPlayer.Dimension);
            obj.SetInvincible(true);
            var mapCreatorObj = new MapCreatorObject(obj, EMapCreatorPositionType.TeamSpawn, editingTeamIndex, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static MapCreatorObject GetMapCenter(int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == EMapCreatorPositionType.MapCenter);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(ClientConstants.MapCenterHash, EMapCreatorPositionType.MapCenter, id: id);
        }

        public static MapCreatorObject GetMapLimit(int id = -1)
        {
            return GetObject(ClientConstants.MapLimitHash, EMapCreatorPositionType.MapLimit, id: id);
        }

        public static MapCreatorObject GetBombPlantPlace(int id = -1)
        {
            return GetObject(ClientConstants.BombPlantPlaceHash, EMapCreatorPositionType.BombPlantPlace, id: id);
        }

        public static MapCreatorObject GetObject(uint hash, EMapCreatorPositionType type, string objName = null, int id = -1)
        {
           
            MapObject obj = new MapObject(hash, Player.LocalPlayer.Position, Player.LocalPlayer.GetRotation(2), dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(obj, type, objectName: objName, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static void DeleteTeamObjects(int teamNumber)
        {
            var teamObjects = _cacheMapEditorObjects.Where(entry => entry.Value.TeamNumber == teamNumber).ToList();
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
            _cacheMapEditorObjects.Remove(obj.Entity);
            obj.Delete();
            ObjectPlacing.CheckObjectDeleted();
        }

        public static void LoadMap(MapCreateDataDto map)
        {
            Stop();

            if (map.BombPlaces != null)
            {
                foreach (var bombPlace in map.BombPlaces)
                {
                    var obj = GetBombPlantPlace(bombPlace.Id);
                    obj.LoadPos(bombPlace);
                }
            }

            if (map.MapCenter != null)
            {
                var obj = GetMapCenter(map.MapCenter.Id);
                obj.LoadPos(map.MapCenter);
            }

            if (map.MapEdges != null)
            {
                foreach (var mapEdge in map.MapEdges)
                {
                    var obj = GetMapLimit(mapEdge.Id);
                    obj.LoadPos(mapEdge);
                }
            }

            if (map.Objects != null)
            {
                foreach (var objPos in map.Objects)
                {
                    string objName = Convert.ToString(objPos.Info);
                    uint objectHash = Misc.GetHashKey(objName);
                    var obj = GetObject(objectHash, EMapCreatorPositionType.Object, objName, objPos.Id);
                    obj.LoadPos(objPos);
                }
            }

            if (map.TeamSpawns != null)
            {

                foreach (var teamSpawns in map.TeamSpawns)
                {
                    foreach (var spawnPos in teamSpawns)
                    {
                        var obj = GetTeamSpawn(Convert.ToInt32(spawnPos.Info), spawnPos.Id);
                        obj.LoadPos(spawnPos);
                    }
                }
            }

            Start();
        }


        private static void OnEntityStreamIn(Entity entity)
        {
            GameEntity ent = (GameEntity)entity;
            if (ent == null)
                return;

            var obj = GetByHandle(ent.Handle);
            if (obj == null)
                return;

            obj.MovingRotation = obj.Rotation;
        }
    }
}
