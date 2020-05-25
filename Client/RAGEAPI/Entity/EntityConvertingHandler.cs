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
        #region Private Fields

        private readonly Dictionary<RAGE.Elements.Entity, IEntity> _entitiesCache = new Dictionary<RAGE.Elements.Entity, IEntity>();

        private PlayerConvertingHandler _playerConvertingHandler;

        #endregion Private Fields

        #region Public Methods

        public IBlip GetEntity(RAGE.Elements.Blip modBlip)
        {
            if (modBlip is null)
                return null;

            if (!_entitiesCache.TryGetValue(modBlip, out IEntity entity))
            {
                entity = new Blip.Blip(modBlip);
                _entitiesCache.Add(modBlip, entity);
            }

            return (IBlip)entity;
        }

        public IMapObject GetEntity(RAGE.Elements.MapObject mapObject)
        {
            if (mapObject is null)
                return null;

            if (!_entitiesCache.TryGetValue(mapObject, out IEntity entity))
            {
                entity = new MapObject.MapObject(mapObject);
                _entitiesCache.Add(mapObject, entity);
            }

            return (IMapObject)entity;
        }

        public IPed GetEntity(RAGE.Elements.Ped ped)
        {
            if (ped is null)
                return null;

            if (!_entitiesCache.TryGetValue(ped, out IEntity entity))
            {
                entity = new Ped.Ped(ped);
                _entitiesCache.Add(ped, entity);
            }

            return (IPed)entity;
        }

        public IVehicle GetEntity(RAGE.Elements.Vehicle veh)
        {
            if (veh is null)
                return null;

            if (!_entitiesCache.TryGetValue(veh, out IEntity entity))
            {
                entity = new Vehicle.Vehicle(veh, this);
                _entitiesCache.Add(veh, entity);
            }

            return (IVehicle)entity;
        }

        public IPlayer GetEntity(RAGE.Elements.Player modPlayer)
            => modPlayer != null ? _playerConvertingHandler.GetPlayer(modPlayer) : null;

        public IEntity GetEntity(RAGE.Elements.GameEntity en)
        {
            if (en is null)
                return null;

            if (!_entitiesCache.TryGetValue(en, out IEntity entity))
            {
                entity = new Entity(en);
                _entitiesCache.Add(en, entity);
            }

            return (IVehicle)entity;
        }

        public IEntity GetEntity(RAGE.Elements.Entity en)
        {
            if (en is null)
                return null;

            if (en is RAGE.Elements.Player player)
                return GetEntity(player);

            if (en is RAGE.Elements.Vehicle vehicle)
                return GetEntity(vehicle);

            if (en is RAGE.Elements.MapObject mapObject)
                return GetEntity(mapObject);

            if (en is RAGE.Elements.Ped ped)
                return GetEntity(ped);

            if (en is RAGE.Elements.Blip blip)
                return GetEntity(blip);

            return null;
        }

        public void SetPlayerConvertingHandler(PlayerConvertingHandler playerConvertingHandler)
            => _playerConvertingHandler = playerConvertingHandler;

        #endregion Public Methods
    }
}
