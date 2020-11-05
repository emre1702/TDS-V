using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Database.SeedData.Gang
{
    public static class GangRanksSeeds
    {
        public static ModelBuilder HasGangRanks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GangRanks>().HasData(
                new GangRanks { Id = -1, GangId = -1, Rank = 0, Name = "-", Color = "rgb(255,255,255)" }
            );
            return modelBuilder;
        }
    }
}
