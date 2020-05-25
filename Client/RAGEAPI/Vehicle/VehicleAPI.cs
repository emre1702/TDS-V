using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Vehicle
{
    internal class VehicleAPI : IVehicleAPI
    {
        #region Public Constructors

        public VehicleAPI()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IVehicle Create(uint hash, Position3D position, Position3D rotation, string numberPlate = "TDS-V", int alpha = 255, bool locked = false,
            int primColor = 0, int secColor = 1, uint dimension = 0)
        {
            var instance = new RAGE.Elements.Vehicle(hash, position.ToVector3(), rotation.Z, numberPlate, alpha, locked, primColor, secColor, dimension);
            return instance as IVehicle;
        }

        public void SetVehicleOnGroundProperly(int handle)
        {
            RAGE.Game.Vehicle.SetVehicleOnGroundProperly(handle, 5);
        }

        #endregion Public Methods
    }
}
