using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class VehiclePaintExtension
    {
        #region Public Methods

        public static VehiclePaint ToMod(this Data.Models.GTA.VehiclePaint color)
        {
            return new VehiclePaint(color.Type, color.Color);
        }

        public static Data.Models.GTA.VehiclePaint ToTDS(this VehiclePaint color)
        {
            return new Data.Models.GTA.VehiclePaint(color.Type, color.Color);
        }

        #endregion Public Methods
    }
}
