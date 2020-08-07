using System;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangwarAreas
    {
        public int MapId { get; set; }
        public int OwnerGangId { get; set; }
        public int AttackCount { get; set; }
        // Amount defend since last capture
        public int DefendCount { get; set; }
        public DateTime LastAttacked { get; set; }


        public virtual Maps Map { get; set; }
        public virtual Gangs OwnerGang { get; set; }
    }
}
