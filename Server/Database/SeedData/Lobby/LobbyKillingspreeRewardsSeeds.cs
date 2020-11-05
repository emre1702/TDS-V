using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbyKillingspreeRewardsSeeds
    {
        public static ModelBuilder HasLobbyKillingspreeRewards(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyKillingspreeRewards>().HasData(
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 3, HealthOrArmor = 30 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 5, HealthOrArmor = 50 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 10, HealthOrArmor = 100 },
                new LobbyKillingspreeRewards { LobbyId = -1, KillsAmount = 15, HealthOrArmor = 100 }
            );
            return modelBuilder;
        }
    }
}
