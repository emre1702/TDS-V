using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Pool;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolVehiclesAPI : IPoolVehiclesAPI
    {
        #region Public Constructors

        public PoolVehiclesAPI()
        {
            RAGE.Entities.Vehicles.CreateEntity = netHandle => new Vehicle.Vehicle(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IVehicle> All => RAGE.Entities.Vehicles.All.OfType<IVehicle>().ToList();
        public IEnumerable<IVehicle> AsEnumerable => RAGE.Entities.Vehicles.AsEnumerable.OfType<IVehicle>();
        public int Count => RAGE.Entities.Vehicles.Count;

        #endregion Public Properties

        #region Public Methods

        public IVehicle? GetAt(ushort id)
            => RAGE.Entities.Vehicles.GetAt(id) as IVehicle;

        #endregion Public Methods
    }
}
