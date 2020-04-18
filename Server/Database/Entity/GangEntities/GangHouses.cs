﻿using System;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangHouses
    {
        public int Id { get; set; }
        public byte NeededGangLevel { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public float Rot { get; set; }
        public DateTime? LastBought { get; set; }
        public int CreatorId { get; set; }
        public DateTime Created { get; set; }

        public virtual Gangs OwnerGang { get; set; }
        public virtual Players Creator { get; set; }
    }
}