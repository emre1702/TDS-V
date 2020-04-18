using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangLevelSettings
    {
        public byte Level { get; set; }
        public int UpgradePrice { get; set; }
        public int HousePrice { get; set; }
        public int NeededExperience { get; set; }

        public byte PlayerSlots { get; set; }
        public byte RankSlots { get; set; }
        public byte VehicleSlots { get; set; }
        public byte GangAreaSlots { get; set; }
        public bool CanChangeBlipColor { get; set; }

    }
}
