using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicleAPI
    {
        IVehicle Create(VehicleHash vehHash, Position3D pos, float rotation, int color1, int color2, string numberPlate, uint dimension);
    }
}
