using System;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangwarAreas
    {
        #region Public Properties

        public int AttackCount { get; set; }
        public int DefendCount { get; set; }
        public DateTime LastAttacked { get; set; }
        public virtual Maps Map { get; set; }
        public int MapId { get; set; }

        // Amount defend since last capture
        public virtual Gangs OwnerGang { get; set; }

        public int OwnerGangId { get; set; }

        #endregion Public Properties
    }
}
