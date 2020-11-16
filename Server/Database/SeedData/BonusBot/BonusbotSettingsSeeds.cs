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
                    GuildId = 320309924175282177,
                    AdminApplicationsChannelId = 659072893526736896,
                    SupportRequestsChannelId = 659073029896142855,
                    ServerInfosChannelId = 659073271911809037,
                    ActionsInfoChannelId = 659088752890871818,
                    BansInfoChannelId = 659705941771550730,

                    ErrorLogsChannelId = 659073884796092426,

                    SendPrivateMessageOnBan = true,
                    SendPrivateMessageOnOfflineMessage = true
                }
            );
            return modelBuilder;
        }
    }
}
