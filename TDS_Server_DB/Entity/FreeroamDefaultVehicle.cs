using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class FreeroamDefaultVehicle
    {
        public short VehicleTypeId { get; set; }
        public int VehicleHash { get; set; }
        public string Note { get; set; }

        public virtual FreeroamVehicleType VehicleType { get; set; }
    }
}
