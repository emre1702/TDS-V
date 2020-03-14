using GTANetworkAPI;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class FreeroamDefaultVehicle
    {
        public FreeroamVehicleType VehicleType { get; set; }
        public VehicleHash VehicleHash { get; set; }
        public string Note { get; set; }
    }
}
