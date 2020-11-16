using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.SeedData.Gang
{
    public static class GangsSeeds
    {
        public static ModelBuilder HasGangs(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gangs>().HasData(
                new Gangs { Id = -1, TeamId = -5, Name = "System", Short = "-", Color = "rgb(255,255,255)" }
            );
            return modelBuilder;
        }
    }
}
