using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.SeedData.Lobby
{
    public static class LobbyMapSettingsSeeds
    {
        public static ModelBuilder HasLobbyMapSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyMapSettings>().HasData(
                new LobbyMapSettings { LobbyId = -1, MapLimitTime = 10, MapLimitType = MapLimitType.KillAfterTime }
            );
            return modelBuilder;
        }
    }
}
