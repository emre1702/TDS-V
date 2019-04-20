using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class KillingspreeRewards
    {
        public int KillsAmount { get; set; }
        public short? HealthOrArmor { get; set; }
        public short? OnlyHealth { get; set; }
        public short? OnlyArmor { get; set; }
    }
}
