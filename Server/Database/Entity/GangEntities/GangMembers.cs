﻿using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.GangEntities
{
    public class GangMembers
    {
        public int GangId { get; set; }
        public DateTime JoinTime { get; set; }
        public int PlayerId { get; set; }
        public int RankId { get; set; }
        public short? RankNumber { get; set; }
        public string Name { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual Gangs Gang { get; set; }
        public virtual Players Player { get; set; }
        public virtual GangRanks Rank { get; set; }
    }
}
