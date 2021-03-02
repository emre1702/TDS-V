using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Bonusbot;

namespace TDS.Server.Database.SeedData.BonusBot
{
    public static class BonusbotSettingsSeeds
    {
        public static ModelBuilder HasBonusBotSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BonusbotSettings>().HasData(
                new BonusbotSettings
                {
                    Id = 1,

                    SendPrivateMessageOnBan = true,
                    SendPrivateMessageOnOfflineMessage = true,
                    RefreshServerStatsFrequencySec = 300
                }
            );
            return modelBuilder;
        }
    }
}