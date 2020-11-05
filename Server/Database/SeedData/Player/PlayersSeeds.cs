using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.SeedData.Player
{
    public static class PlayersSeeds
    {
        public static ModelBuilder HasPlayers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Players>().HasData(
                new Players { Id = -1, SCName = "System", SCId = 0, Name = "System", AdminLeaderId = -1, Password = "" }
            );
            return modelBuilder;
        }
    }
}
