using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class KillingspreeRewards
    {
        public int KillsAmount { get; set; }
        public byte HealthOrArmor { get; set; }
        public byte OnlyHealth { get; set; }
        public byte OnlyArmor { get; set; }
    }
}
