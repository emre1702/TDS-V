﻿using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server_DB.Entity.Gang
{
    public class GangwarAreas
    {
        public int MapId { get; set; }
        public int OwnerGangId { get; set; }
        public DateTime LastAttacked { get; set; }

        public int AttackCount { get; set; }
        public int DefendCount { get; set; }    // Amount defend since last capture


        public virtual Maps Map { get; set; }
        public virtual Gangs OwnerGang { get; set; }
    }
}
