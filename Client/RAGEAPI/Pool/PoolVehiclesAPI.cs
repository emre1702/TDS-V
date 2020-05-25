using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Interfaces.ModAPI.Pool;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Client.RAGEAPI.Pool
{
    internal class PoolVehiclesAPI : IPoolVehiclesAPI
    {
        #region Public Constructors

        public PoolVehiclesAPI()
        {
            RAGE.Elements.Entities.Vehicles.CreateEntity = (ushort id, ushort remoteId) => new Vehicle.Vehicle(id, remoteId);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IVehicle> All
            => RAGE.Elements.Entities.Vehicles.All.OfType<IVehicle>().ToList();

        public List<IVehicle> Streamed
            => RAGE.Elements.Entities.Vehicles.Streamed.OfType<IVehicle>().ToList();

        #endregion Public Properties

        #region Public Methods

        public IVehicle GetAt(ushort id)
            => RAGE.Elements.Entities.Vehicles.GetAt(id) as IVehicle;

        public IVehicle GetAtHandle(int handle)
             => RAGE.Elements.Entities.Vehicles.GetAtHandle(handle) as IVehicle;

        public IVehicle GetAtRemote(ushort handleValue)
            => RAGE.Elements.Entities.Vehicles.GetAtRemote(handleValue) as IVehicle;

        #endregion Public Methods
    }
}
