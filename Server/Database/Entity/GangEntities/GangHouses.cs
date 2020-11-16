using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.GangEntities
{
    public class GangHouses
    {
        #region Public Properties

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastBought { get; set; }
        public byte NeededGangLevel { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public float Rot { get; set; }

        public virtual Gangs OwnerGang { get; set; }
        public virtual Players Creator { get; set; }

        #endregion Public Properties
    }
}
