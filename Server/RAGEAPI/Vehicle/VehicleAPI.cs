using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Vehicle
{
    class VehicleAPI : IVehicleAPI
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        internal VehicleAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        public IVehicle Create(VehicleHash vehHash, Position3D pos, float rotation, int color1, int color2, string numberPlate, uint dimension)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle((GTANetworkAPI.VehicleHash)vehHash, pos.ToVector3(), rotation, color1, color2, numberPlate, dimension: dimension);

            return _entityConvertingHandler.GetEntity(instance);
        }
    }
}
