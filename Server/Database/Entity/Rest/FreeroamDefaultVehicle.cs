using GTANetworkAPI;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Rest
{
    public partial class FreeroamDefaultVehicle
    {
        #region Public Properties

        public string Note { get; set; }
        public VehicleHash VehicleHash { get; set; }
        public FreeroamVehicleType VehicleType { get; set; }

        #endregion Public Properties
    }
}
