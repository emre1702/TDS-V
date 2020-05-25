using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.RAGEAPI.Entity;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolAPI : IPoolAPI
    {
        #region Public Constructors

        public PoolAPI()
        {
            Objects = new PoolObjectsAPI();
            Peds = new PoolPedsAPI();
            Players = new PoolPlayersAPI();
            Vehicles = new PoolVehiclesAPI();
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
