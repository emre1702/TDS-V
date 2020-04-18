using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangStats
    {
        public int GangId { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int AmountAttacks { get; set; }
        public int AmountDefends { get; set; }
        public int AmountAttacksWon { get; set; }
        public int AmountDefendsWon { get; set; }
        public int AmountMembersSoFar { get; set; }
        public int PeakGangwarAreasOwned { get; set; }
        public int TotalMoneySoFar { get; set; }

        public Gangs Gang { get; set; }
    }
}
