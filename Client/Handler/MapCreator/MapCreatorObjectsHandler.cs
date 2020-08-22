using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObjectsHandler : ServiceBase
    {
        #region Public Fields

        public MapLimit MapLimitDisplay;

        #endregion Public Fields

        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly Dictionary<IEntityBase, MapCreatorObject> _cacheMapEditorObjects = new Dictionary<IEntityBase, MapCreatorObject>();
        private readonly CamerasHandler _camerasHandler;
        private readonly EventMethodData<EntityStreamInDelegate> _entityStreamInEventMethod;
        private readonly EventsHandler _eventsHandler;

        //private readonly EventMethodData<EntityStreamOutDelegate> _entityStreamOutEventMethod;
        private readonly LobbyHandler _lobbyHandler;

        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public MapCreatorObjectsHandler(IModAPI modAPI, LoggingHandler loggingHandler, CamerasHandler camerasHandler, LobbyHandler lobbyHandler,
            EventsHandler eventsHandler, BrowserHandler browserHandler, Serializer serializer)
            : base(modAPI, loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _lobbyHandler = lobbyHandler;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;
            _serializer = serializer;

            _entityStreamInEventMethod = new EventMethodData<EntityStreamInDelegate>(OnEntityStreamIn);
            //_entityStreamOutEventMethod = new EventMethodData<EntityStreamOutDelegate>(OnEntityStreamOut);

            eventsHandler.MapBorderColorChanged += EventsHandler_MapBorderColorChanged;

            modAPI.Event.Add(FromBrowserEvent.RemoveMapCreatorPosition, OnRemoveMapCreatorPositionMethod);
            modAPI.Event.Add(FromBrowserEvent.RemoveMapCreatorTeamNumber, OnRemoveMapCreatorTeamNumberMethod);
            modAPI.Event.Add(ToClientEvent.LoadMapForMapCreator, OnLoadMapForMapCreatorServerMethod);
        }

        #endregion Public Constructors

        #region Public Methods

        public bool CanEditObject(MapCreatorObject obj)
        {
            return obj.OwnerRemoteId == ModAPI.LocalPlayer.RemoteId || _lobbyHandler.IsLobbyOwner;
        }

        public MapCreatorObject CreateMapCreatorObject(MapCreatorPositionType type, object info, ushort ownerRemoteId,
            Position pos, Position rot)
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
                        Position.GetPos(data), Position.GetRot(data), id: data.Id);

                case MapCreatorPositionType.MapCenter:
                    return GetMapCenter(data.OwnerRemoteId, Position.GetPos(data), Position.GetRot(data), id: data.Id);

                case MapCreatorPositionType.BombPlantPlace:
                    return GetBombPlantPlace(data.OwnerRemoteId, Position.GetPos(data), Position.GetRot(data), id: data.Id);

                case MapCreatorPositionType.MapLimit:
                    return GetMapLimit(data.OwnerRemoteId, Position.GetPos(data), Position.GetRot(data), id: data.Id);

                case MapCreatorPositionType.Target:
                    return GetTarget(data.OwnerRemoteId, Position.GetPos(data), Position.GetRot(data), id: data.Id);

                case MapCreatorPositionType.Object:
                    string objName = (string)data.Info;
                    return GetObject(objName, MapCreatorPositionType.Object, data.OwnerRemoteId,
                        Position.GetPos(data), Position.GetRot(data), objName, id: data.Id);

                case MapCreatorPositionType.Vehicle:
                    string vehName = (string)data.Info;
                    return GetVehicle(vehName, data.OwnerRemoteId, vehName: vehName, id: data.Id,
                        pos: Position.GetPos(data), rot: Position.GetRot(data));
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

        public MapCreatorObject GetBombPlantPlace(ushort playerRemoteId, Position pos, Position rot, int id = -1)
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

        public MapCreatorObject GetMapCenter(ushort playerRemoteId, Position pos, Position rot, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.MapCenter);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.MapCenterHashName, MapCreatorPositionType.MapCenter, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetMapLimit(ushort playerRemoteId, Position pos, Position rot, int id = -1)
        {
            return GetObject(Constants.MapLimitHashName, MapCreatorPositionType.MapLimit, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetObject(string hashName, MapCreatorPositionType type, ushort playerRemoteId,
            Position pos, Position rot, string objName = null, int id = -1)
        {
            uint hash = ModAPI.Misc.GetHashKey(hashName);
            var obj = ModAPI.MapObject.Create(hash, pos, rot, dimension: ModAPI.LocalPlayer.Dimension);
            var mapCreatorObj = new MapCreatorObject(ModAPI, this, _eventsHandler, obj, type, playerRemoteId, pos, rot, objectName: objName, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetOrCreateByHandle(int handle, MapCreatorPositionType? type = null)
        {
            var alreadyCached = GetByHandle(handle);
            if (alreadyCached != null)
                return alreadyCached;
            var entityType = ModAPI.Entity.GetEntityType(handle);

            IEntityBase entity;
            switch (entityType)
            {
                case EntityType.Ped:
                    entity = ModAPI.Pool.Peds.GetAtHandle(handle);
                    break;

                case EntityType.Vehicle:
                    entity = ModAPI.Pool.Vehicles.GetAtHandle(handle);
                    break;

                case EntityType.Object:
                    entity = ModAPI.Pool.Objects.GetAtHandle(handle);
                    break;

                default:
                    return null;
            }

            if (!type.HasValue)
            {
                switch (ModAPI.Entity.GetEntityType(handle))
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

            var obj = new MapCreatorObject(ModAPI, this, _eventsHandler, entity, type.Value, ModAPI.LocalPlayer.RemoteId, entity.Position, entity.Rotation);
            _cacheMapEditorObjects[entity] = obj;
            return obj;
        }

        public MapCreatorObject GetTarget(ushort playerRemoteId, Position pos, Position rot, int id = -1)
        {
            var entry = _cacheMapEditorObjects.FirstOrDefault(e => e.Value.Type == MapCreatorPositionType.Target);
            if (entry.Value != null)
                return entry.Value;
            return GetObject(Constants.TargetHashName, MapCreatorPositionType.Target, playerRemoteId, pos, rot, id: id);
        }

        public MapCreatorObject GetTeamSpawn(int editingTeamIndex, ushort playerRemoteId, Position pos, Position rot, int id = -1)
        {
            int pedHashIndex = editingTeamIndex;
            while (pedHashIndex >= Constants.TeamSpawnPedHash.Length)
                pedHashIndex -= Constants.TeamSpawnPedHash.Length;
            var obj = ModAPI.Ped.Create(Constants.TeamSpawnPedHash[pedHashIndex], pos, rot, dimension: ModAPI.LocalPlayer.Dimension);
            obj.SetInvincible(true);
            obj.FreezePosition(true);
            var mapCreatorObj = new MapCreatorObject(ModAPI, this, _eventsHandler, obj, MapCreatorPositionType.TeamSpawn, playerRemoteId, pos, rot, editingTeamIndex, id: id);
            _cacheMapEditorObjects[obj] = mapCreatorObj;
            return mapCreatorObj;
        }

        public MapCreatorObject GetVehicle(string hashName, ushort playerRemoteId, Position pos, Position rot, string vehName = null, int id = -1)
        {
            uint hash = ModAPI.Misc.GetHashKey(hashName);
            var vehicle = ModAPI.Vehicle.Create(hash, pos, rot, "Map",
                locked: true, dimension: ModAPI.LocalPlayer.Dimension);
            vehicle.FreezePosition(true);
            //vehicle.SetCollision(false, true);
            var mapCreatorObj = new MapCreatorObject(ModAPI, this, _eventsHandler, vehicle, MapCreatorPositionType.Vehicle, playerRemoteId, pos, rot, objectName: vehName, id: id);
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
                var obj = GetMapCenter(map.MapCenter.OwnerRemoteId, Position.GetPos(map.MapCenter), Position.GetRot(map.MapCenter), map.MapCenter.Id);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = Position.GetPos(map.MapCenter);
                else
                    ModAPI.LocalPlayer.Position = Position.GetPos(map.MapCenter);
            }

            if (map.Target != null)
            {
                var obj = GetTarget(map.Target.OwnerRemoteId, Position.GetPos(map.Target), Position.GetRot(map.Target), map.Target.Id);
                if (_camerasHandler.ActiveCamera != null)
                    _camerasHandler.ActiveCamera.Position = Position.GetPos(map.Target);
                else
                    ModAPI.LocalPlayer.Position = Position.GetPos(map.Target);
            }

            if (map.BombPlaces != null)
            {
                foreach (var bombPlace in map.BombPlaces)
                {
                    var obj = GetBombPlantPlace(bombPlace.OwnerRemoteId, Position.GetPos(bombPlace), Position.GetRot(bombPlace), bombPlace.Id);
                }
            }

            if (map.MapEdges != null)
            {
                foreach (var mapEdge in map.MapEdges)
                {
                    var obj = GetMapLimit(mapEdge.OwnerRemoteId, Position.GetPos(mapEdge), Position.GetRot(mapEdge), mapEdge.Id);
                }
            }

            if (map.Objects != null)
            {
                foreach (var objPos in map.Objects)
                {
                    string objName = Convert.ToString(objPos.Info);
                    var obj = GetObject(objName, MapCreatorPositionType.Object, objPos.OwnerRemoteId,
                        Position.GetPos(objPos), Position.GetRot(objPos), objName, objPos.Id);
                }
            }

            if (map.Vehicles != null)
            {
                foreach (var vehPos in map.Vehicles)
                {
                    string vehName = Convert.ToString(vehPos.Info);
                    GetVehicle(vehName, vehPos.OwnerRemoteId, Position.GetPos(vehPos), Position.GetRot(vehPos), vehName, vehPos.Id);
                }
            }

            if (map.TeamSpawns != null)
            {
                foreach (var teamSpawns in map.TeamSpawns)
                {
                    foreach (var spawnPos in teamSpawns)
                    {
                        var obj = GetTeamSpawn(Convert.ToInt32(spawnPos.Info), spawnPos.OwnerRemoteId,
                            Position.GetPos(spawnPos), Position.GetRot(spawnPos), spawnPos.Id);
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
                obj.MovingRotation = new Position(obj.Rotation);
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
            ModAPI.Event.EntityStreamIn.Add(_entityStreamInEventMethod);
            //ModAPI.Event.EntityStreamOut.Add(_entityStreamOutEventMethod);
        }

        public void Stop()
        {
            ModAPI.Event.EntityStreamIn.Remove(_entityStreamInEventMethod);
            //ModAPI.Event.EntityStreamOut.Remove(_entityStreamOutEventMethod);

            foreach (var entry in _cacheMapEditorObjects)
            {
                entry.Value.Delete(false);
            }
            _cacheMapEditorObjects.Clear();
            IdCounter = 0;

            MapLimitDisplay?.Stop();
            MapLimitDisplay = null;
        }

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_MapBorderColorChanged(Color color)
        {
            if (!(MapLimitDisplay is null))
                MapLimitDisplay.MapBorderColor = color;
        }

        private void OnEntityStreamIn(IEntity entity)
        {
            if (!(entity is IMapObject mapObject))
                return;
            var obj = GetByHandle(mapObject.Handle);
            if (obj is null)
                return;
            if (obj.Rotation is null)
                return;

            obj.MovingRotation = new Position(obj.Rotation);
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
            var mapCreatorData = _serializer.FromServer<MapCreateDataDto>(json);
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

        #endregion Private Methods
    }
}
