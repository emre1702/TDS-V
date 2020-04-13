using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolVehiclesAPI : IPoolVehiclesAPI
    {
        public IEnumerable<IVehicle> All
        {
            get
            {
                var list = new List<IVehicle>();
                foreach (var obj in RAGE.Elements.Entities.Vehicles.All)
                {
                    list.Add(_entityConvertingHandler.GetEntity(obj));
                }
                return list;
            }
        }

        public IVehicle GetAtHandle(int handle)
        {
            var obj = RAGE.Elements.Entities.Vehicles.GetAtHandle(handle);
            return _entityConvertingHandler.GetEntity(obj);
        }

        public IVehicle GetAtRemote(ushort handleValue)
        {
            var obj = RAGE.Elements.Entities.Vehicles.GetAtRemote(handleValue);
            return _entityConvertingHandler.GetEntity(obj);
        }

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PoolVehiclesAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;
    }
}
