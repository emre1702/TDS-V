using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Server;

namespace TDS.Server.Database.SeedData.Server
{
    public static class ServerTotalStatsSeeds
    {
        public static ModelBuilder HasServerTotalStats(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerTotalStats>().HasData(
                new ServerTotalStats { Id = 1 }
            );
            return modelBuilder;
        }
    }
}
