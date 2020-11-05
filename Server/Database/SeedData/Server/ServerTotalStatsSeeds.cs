using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Server;

namespace TDS_Server.Database.SeedData.Server
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
