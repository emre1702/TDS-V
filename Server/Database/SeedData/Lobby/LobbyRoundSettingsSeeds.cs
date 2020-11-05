using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.SeedData.Lobby
{
    public static class LobbyRoundSettingsSeeds
    {
        public static ModelBuilder HasLobbyRoundSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LobbyRoundSettings>().HasData(
                new LobbyRoundSettings
                {
                    LobbyId = -1,
                    RoundTime = 240,
                    CountdownTime = 5,
                    BombDetonateTimeMs = 45000,
                    BombDefuseTimeMs = 8000,
                    BombPlantTimeMs = 3000,
                    MixTeamsAfterRound = true,
                    ShowRanking = true
                }
            );
            return modelBuilder;
        }
    }
}
