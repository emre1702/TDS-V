using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Admin;

namespace TDS.Server.Database.SeedData.Admin
{
    public static class AdminLevelsSeeds
    {
        public static ModelBuilder HasAdminLevels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLevels>().HasData(
                new AdminLevels { Level = 0, ColorR = 220, ColorG = 220, ColorB = 220 },
                new AdminLevels { Level = 1, ColorR = 113, ColorG = 202, ColorB = 113 },
                new AdminLevels { Level = 2, ColorR = 253, ColorG = 132, ColorB = 85 },
                new AdminLevels { Level = 3, ColorR = 222, ColorG = 50, ColorB = 50 }
            );
            return modelBuilder;
        }
    }
}
