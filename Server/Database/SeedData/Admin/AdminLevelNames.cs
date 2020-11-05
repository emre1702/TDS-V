using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TDS_Server.Database.Entity.Admin;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Admin
{
    public static class AdminLevelNamesSeeds
    {
        public static ModelBuilder HasAdminLevelNames(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLevelNames>().HasData(
                new AdminLevelNames { Level = 0, Language = Language.English, Name = "User" },
                new AdminLevelNames { Level = 0, Language = Language.German, Name = "User" },
                new AdminLevelNames { Level = 1, Language = Language.English, Name = "Supporter" },
                new AdminLevelNames { Level = 1, Language = Language.German, Name = "Supporter" },
                new AdminLevelNames { Level = 2, Language = Language.English, Name = "Administrator" },
                new AdminLevelNames { Level = 2, Language = Language.German, Name = "Administrator" },
                new AdminLevelNames { Level = 3, Language = Language.English, Name = "Projectleader" },
                new AdminLevelNames { Level = 3, Language = Language.German, Name = "Projektleiter" }
            );
            return modelBuilder;
        }
    }
}
