﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.SeedData.Gang
{
    public static class GangRankPermissionsSeeds
    {
        public static ModelBuilder HasGangRankPermissions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GangRankPermissions>().HasData(
                new GangRankPermissions { GangId = -1, InviteMembers = 1, KickMembers = 1, ManagePermissions = 1, ManageRanks = 1, StartGangAction = 1 }
            );
            return modelBuilder;
        }
    }
}
