using GTANetworkAPI;
using TDS_Common.Enum;

namespace TDS_Server_DB.Entity
{
    public partial class FreeroamDefaultVehicle
    {
        public EFreeroamVehicleType VehicleType { get; set; }
        public VehicleHash VehicleHash { get; set; }
        public string Note { get; set; }
    }
}
