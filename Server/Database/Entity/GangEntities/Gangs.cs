using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.Entity.GangEntities
{
    public partial class Gangs
    {
        #region Public Properties

        public byte BlipColor { get; set; }
        public DateTime CreateTime { get; set; }
        public int? HouseId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OwnerId { get; set; }
        public string Short { get; set; }
        public int TeamId { get; set; }
        public string Color { get; set; }


        public virtual Players Owner { get; set; }
        public virtual GangStats Stats { get; set; }
        public virtual Teams Team { get; set; }
        public virtual GangHouses House { get; set; }
        public virtual ICollection<GangActionAreas> GangwarAreas { get; set; }
        public virtual GangRankPermissions RankPermissions { get; set; }
        public virtual ICollection<GangMembers> Members { get; set; }
        public virtual ICollection<GangRanks> Ranks { get; set; }
        public virtual ICollection<GangVehicles> Vehicles { get; set; }

        #endregion Public Properties
    }
}
