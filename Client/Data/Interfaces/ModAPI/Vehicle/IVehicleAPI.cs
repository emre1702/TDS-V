using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicleAPI
    {
        #region Public Methods

        IVehicle Create(uint hash, Position position, Position rotation, string numberPlate = "TDS-V", int alpha = 255, bool locked = false,
            int primColor = 0, int secColor = 1, uint dimension = 0);

        void SetVehicleOnGroundProperly(int handle);

        #endregion Public Methods
    }
}
