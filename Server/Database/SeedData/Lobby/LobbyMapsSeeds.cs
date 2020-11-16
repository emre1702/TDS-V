using Microsoft.EntityFrameworkCore;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.SeedData.Lobby
{
    public static class LobbyMapsSeeds
    {
        public static ModelBuilder HasLobbyMaps(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyMaps>().HasData(
                new LobbyMaps { LobbyId = -1, MapId = -1 }
            );
            return modelBuilder;
        }
    }
}
