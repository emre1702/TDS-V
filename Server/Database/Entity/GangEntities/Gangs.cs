﻿using System;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.GangEntities
{
    public partial class Gangs
    {

        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Short { get; set; }
        public byte BlipColor { get; set; }
        public int? OwnerId { get; set; }
        public int? HouseId { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ICollection<GangwarAreas> GangwarAreas { get; set; }
        public virtual ICollection<GangMembers> Members { get; set; }
        public virtual Players Owner { get; set; }
        public virtual GangRankPermissions RankPermissions { get; set; }
        public virtual ICollection<GangRanks> Ranks { get; set; }
        public virtual Teams Team { get; set; }
        public virtual GangHouses House { get; set; }
        public virtual GangStats Stats { get; set; }
        public virtual ICollection<GangVehicles> Vehicles { get; set; }
    }
}
