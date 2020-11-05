using Microsoft.EntityFrameworkCore;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbyRewardsSeeds
    {
        public static ModelBuilder HasLobbyRewards(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyRewards>().HasData(
               new LobbyRewards { LobbyId = -1, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 },
               new LobbyRewards { LobbyId = -2, MoneyPerKill = 20, MoneyPerAssist = 10, MoneyPerDamage = 0.1 }
           );
            return modelBuilder;
        }
    }
}
