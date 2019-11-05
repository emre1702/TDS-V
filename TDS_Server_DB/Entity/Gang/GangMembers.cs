﻿using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Gang
{
    public class GangMembers
    {
        public int GangId { get; set; }
        public int PlayerId { get; set; }
        public short Rank { get; set; }
        public DateTime JoinTime { get; set; }

        public virtual Gangs Gang { get; set; }
        public virtual Players Player { get; set; }
        public virtual GangRanks RankNavigation { get; set; }
    }
}
