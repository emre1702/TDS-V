using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class FreeroamVehicleType
    {
        public short Id { get; set; }
        public string Type { get; set; }

        public virtual FreeroamDefaultVehicle FreeroamDefaultVehicle { get; set; }
    }
}
