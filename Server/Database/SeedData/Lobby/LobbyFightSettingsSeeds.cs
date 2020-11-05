using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbyFightSettingsSeeds
    {
        public static ModelBuilder HasLobbyFightSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyFightSettings>().HasData(
                new LobbyFightSettings { LobbyId = -1 },
                new LobbyFightSettings { LobbyId = -6 }
            );
            return modelBuilder;
        }
    }
}
