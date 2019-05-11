using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class KillingspreeRewards
    {
        public short KillsAmount { get; set; }
        public short? HealthOrArmor { get; set; }
        public short? OnlyHealth { get; set; }
        public short? OnlyArmor { get; set; }
    }
}
