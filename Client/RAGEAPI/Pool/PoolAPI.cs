using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    class PoolAPI : IPoolAPI
    {
        public IPoolObjectsAPI Objects { get; }

        public IPoolPedsAPI Peds { get; }

        public IPoolPlayersAPI Players { get; }

        public IPoolVehiclesAPI Vehicles { get; }

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PoolAPI(EntityConvertingHandler entityConvertingHandler)
        {
            _entityConvertingHandler = entityConvertingHandler;

            Objects = new PoolObjectsAPI(entityConvertingHandler);
            Peds = new PoolPedsAPI(entityConvertingHandler);
            Players = new PoolPlayersAPI(entityConvertingHandler);
            Vehicles = new PoolVehiclesAPI(entityConvertingHandler);
        }
    }
}
