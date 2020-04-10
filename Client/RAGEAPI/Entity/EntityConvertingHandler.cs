using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.RAGEAPI.Entity
{
    public class EntityConvertingHandler
    {
        private readonly Dictionary<RAGE.Elements.Entity, IEntity> _entitiesCache = new Dictionary<RAGE.Elements.Entity, IEntity>();

        public IEntity GetEntity(RAGE.Elements.Entity modEntity)
        {
            if (!_entitiesCache.TryGetValue(modEntity, out IEntity entity))
            {
                if (modEntity is RAGE.Elements.Player player)
                    entity = new Player.Player(player);
                else if (modEntity is RAGE.Elements.Vehicle vehicle)
                    entity = new Vehicle.Vehicle(vehicle);
                else if (modEntity is RAGE.Elements.Blip blip)
                    entity = new Blip.Blip(blip);
                else if (modEntity is RAGE.Elements.Camera camera)
                    entity = new Camera.Camera(camera);
                else if (modEntity is RAGE.Elements.Marker marker)
                    entity = new Marker.Marker(marker);
                else if (modEntity is RAGE.Elements.Ped ped)
                    entity = new Ped.Ped(ped);
                else if (modEntity is RAGE.Elements.Pickup pickup)
                    entity = new Pickup.Pickup(pickup);
                else 
                    entity = new Entity(modEntity);
                _entitiesCache.Add(modEntity, entity);
            }
            return entity;
        }
    }
}
