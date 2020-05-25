using System;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangHouses
    {
        #region Public Properties

        public DateTime Created { get; set; }
        public virtual Players Creator { get; set; }
        public int CreatorId { get; set; }
        public int Id { get; set; }
        public DateTime? LastBought { get; set; }
        public byte NeededGangLevel { get; set; }

        public virtual Gangs OwnerGang { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public float Rot { get; set; }

        #endregion Public Properties
    }
}
