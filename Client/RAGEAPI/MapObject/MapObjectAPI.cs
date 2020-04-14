﻿using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.MapObject
{
    class MapObjectAPI : IMapObjectAPI
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public MapObjectAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        public IMapObject Create(uint hash, Position3D position, Position3D rotation, int alpha = 255, uint dimension = 0)
        {
            var instance = new RAGE.Elements.MapObject(hash, position.ToVector3(), rotation.ToVector3(), alpha, dimension);
            return _entityConvertingHandler.GetEntity(instance);
        }

        public void PlaceObjectOnGroundProperly(int handle)
        {
            RAGE.Game.Object.PlaceObjectOnGroundProperly(handle);
        }
    }
}