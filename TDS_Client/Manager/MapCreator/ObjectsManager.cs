using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectsManager
    {
        private static readonly Dictionary<GameEntity, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<GameEntity, MapCreatorObject>();

        public static void Add(GameEntity obj)
        {
            var mapCreatorObject = new MapCreatorObject(obj);
            _cacheMapEditorObjects[obj] = mapCreatorObject;
        }

        public static MapCreatorObject GetByHandle(int handle)
        {
            return _cacheMapEditorObjects.FirstOrDefault(g => g.Key.Handle == handle).Value;
        }

        public static MapCreatorObject GetOrCreateByHandle(int handle)
        {
            var alreadyCached = GetByHandle(handle);
            if (alreadyCached != null)
                return alreadyCached;
            var entityType = (EEntityType) RAGE.Game.Entity.GetEntityType(handle);

            GameEntity entity;
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

            var obj = new MapCreatorObject(entity);
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public static MapCreatorObject GetTeamSpawn(int editingTeamIndex)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= ClientConstants.TeamSpawnPedHash.Length) 
                pedHashIndex -= ClientConstants.TeamSpawnPedHash.Length;
            var obj = new Ped(ClientConstants.TeamSpawnPedHash[pedHashIndex], new Vector3(), 0, dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(obj);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static MapCreatorObject GetMapCenter()
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Key.Model == ClientConstants.MapCenterHash);
            if (!entry.Equals(default))
                return entry.Value;
            return GetObject(ClientConstants.MapCenterHash);
        }

        public static MapCreatorObject GetMapLimit()
        {
            return GetObject(ClientConstants.MapLimitHash);
        }

        public static MapCreatorObject GetBombPlantPlace()
        {
            return GetObject(ClientConstants.BombPlantPlaceHash);
        }

        public static MapCreatorObject GetObject(uint hash)
        {
            var obj = new MapObject(hash, new Vector3(), new Vector3(), dimension: Player.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(obj);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public static void Delete(MapCreatorObject obj)
        {
            _cacheMapEditorObjects.Remove(obj.Entity);
            obj.Delete();

        }

        public static void Clear()
        {
            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Key.Destroy();
            }
            _cacheMapEditorObjects.Clear();
        }
    }
}
