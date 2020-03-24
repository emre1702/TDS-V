using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Vehicle
{
    class VehicleAPI : IVehicleAPI
    {
        public IVehicle Create(VehicleHash vehHash, Position3D pos, float rotation, int v1, int v2, string name, uint dimension)
        {
            throw new System.NotImplementedException();
        }
    }
}
