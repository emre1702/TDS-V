using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolAPI : IPoolAPI
    {
        #region Private Fields

        private readonly EntityConvertingHandler _entityConvertingHandler;

        #endregion Private Fields

        #region Public Constructors

        public PoolAPI(EntityConvertingHandler entityConvertingHandler)
        {
            _entityConvertingHandler = entityConvertingHandler;

            Objects = new PoolObjectsAPI(entityConvertingHandler);
            Peds = new PoolPedsAPI(entityConvertingHandler);
            Players = new PoolPlayersAPI(entityConvertingHandler);
            Vehicles = new PoolVehiclesAPI(entityConvertingHandler);
        }

        #endregion Public Constructors

        #region Public Properties

        public IPoolObjectsAPI Objects { get; }

        public IPoolPedsAPI Peds { get; }

        public IPoolPlayersAPI Players { get; }

        public IPoolVehiclesAPI Vehicles { get; }

        #endregion Public Properties
    }
}
