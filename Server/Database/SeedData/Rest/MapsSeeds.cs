using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.SeedData.Rest
{
    public static class MapsSeeds
    {
        public static ModelBuilder HasMaps(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maps>().HasData(
                new Maps { Id = -6, Name = "All Arms Races", CreatorId = -1 },
                new Maps { Id = -5, Name = "All Gangwars", CreatorId = -1 },
                new Maps { Id = -4, Name = "All Sniper", CreatorId = -1 },
                new Maps { Id = -3, Name = "All Bombs", CreatorId = -1 },
                new Maps { Id = -2, Name = "All Normals", CreatorId = -1 },
                new Maps { Id = -1, Name = "All", CreatorId = -1 }
            );
            return modelBuilder;
        }
    }
}
