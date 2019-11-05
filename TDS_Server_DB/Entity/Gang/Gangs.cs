using System;
using System.Collections.Generic;
using TDS_Server_DB.Entity.Player;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server_DB.Entity.Gang
{
    public partial class Gangs
    {
        public Gangs()
        {
            Players = new HashSet<Players>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Short { get; set; }
        public int? OwnerId { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ICollection<GangwarAreas> GangwarAreas { get; set; }
        public virtual ICollection<GangMembers> Members { get; set; }
        public virtual Players Owner { get; set; }
        public virtual ICollection<Players> Players { get; set; }
        public virtual GangRankPermissions RankPermissions { get; set; }
        public virtual ICollection<GangRanks> Ranks { get; set; }
        public virtual Teams Team { get; set; }
    }
}
