using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class VehiclePaintExtension
    {
        public static VehiclePaint ToMod(this Data.Models.GTA.VehiclePaint color)
        {
            return new VehiclePaint(color.Type, color.Color);
        }

        public static Data.Models.GTA.VehiclePaint ToTDS(this VehiclePaint color)
        {
            return new Data.Models.GTA.VehiclePaint(color.Type, color.Color);
        }
    }
}
