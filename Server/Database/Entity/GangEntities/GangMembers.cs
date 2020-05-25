using System;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangMembers
    {
        #region Public Properties

        public virtual Gangs Gang { get; set; }
        public int GangId { get; set; }
        public DateTime JoinTime { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public short Rank { get; set; }
        public virtual GangRanks RankNavigation { get; set; }

        #endregion Public Properties
    }
}
