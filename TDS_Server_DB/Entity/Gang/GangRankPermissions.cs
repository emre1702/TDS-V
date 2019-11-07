﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server_DB.Entity.Gang
{
    public class GangRankPermissions
    {
        public int GangId { get; set; }

        // Administration //
        public short ManagePermissions { get; set; }
        public short InviteMembers { get; set; }
        public short KickMembers { get; set; }
        public short ManageRanks { get; set; }

        // Action //
        public short StartGangwar { get; set; }

        public virtual Gangs Gang { get; set; }
    }
}