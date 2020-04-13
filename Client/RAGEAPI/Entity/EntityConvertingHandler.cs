using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Entity
{
    public class EntityConvertingHandler
    {
        private readonly Dictionary<RAGE.Elements.Entity, IEntity> _entitiesCache = new Dictionary<RAGE.Elements.Entity, IEntity>();

        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public EntityConvertingHandler(PlayerConvertingHandler playerConvertingHandler)
            => _playerConvertingHandler = playerConvertingHandler;

        public IBlip GetEntity(RAGE.Elements.Blip modBlip)
        {
            if (!_entitiesCache.TryGetValue(modBlip, out IEntity entity))
            {
                entity = new Blip.Blip(modBlip);
                _entitiesCache.Add(modBlip, entity);
            }

            return (IBlip)entity;
        }

        public IMapObject GetEntity(RAGE.Elements.MapObject mapObject)
        {
            if (!_entitiesCache.TryGetValue(mapObject, out IEntity entity))
            {
                entity = new MapObject.MapObject(mapObject);
                _entitiesCache.Add(mapObject, entity);
            }

            return (IMapObject)entity;
        }

        public IPed GetEntity(RAGE.Elements.Ped ped)
        {
            if (!_entitiesCache.TryGetValue(ped, out IEntity entity))
            {
                entity = new Ped.Ped(ped);
                _entitiesCache.Add(ped, entity);
            }

            return (IPed)entity;
        }

        public IVehicle GetEntity(RAGE.Elements.Vehicle veh)
        {
            if (!_entitiesCache.TryGetValue(veh, out IEntity entity))
            {
                entity = new Vehicle.Vehicle(veh);
                _entitiesCache.Add(veh, entity);
            }

            return (IVehicle)entity;
        }

        public IPlayer GetEntity(RAGE.Elements.Player modPlayer)
            => _playerConvertingHandler.GetPlayer(modPlayer);

        public IEntity GetEntity(RAGE.Elements.GameEntity en)
        {
            if (!_entitiesCache.TryGetValue(en, out IEntity entity))
            {
                entity = new Entity(en);
                _entitiesCache.Add(en, entity);
            }

            return (IVehicle)entity;
        }

        public IEntity GetEntity(RAGE.Elements.Entity en)
        {
            return null;
        }
    }
}
