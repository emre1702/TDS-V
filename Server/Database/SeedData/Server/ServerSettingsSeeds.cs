using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Server;

namespace TDS.Server.Database.SeedData.Server
{
    public static class ServerSettingsSeeds
    {
        public static ModelBuilder HasServerSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerSettings>().HasData(new ServerSettings
            {
                Id = 1,
                GamemodeName = "tdm",
                ErrorToPlayerOnNonExistentCommand = true,
                ToChatOnNonExistentCommand = false,
                DistanceToSpotToPlant = 3,
                DistanceToSpotToDefuse = 3,
                SavePlayerDataCooldownMinutes = 1,
                SaveLogsCooldownMinutes = 1,
                SaveSeasonsCooldownMinutes = 1,
                TeamOrderCooldownMs = 3000,
                ArenaNewMapProbabilityPercent = 2,
                KillingSpreeMaxSecondsUntilNextKill = 18,
                MapRatingAmountForCheck = 10,
                MinMapRatingForNewMaps = 3f,
                GiveMoneyFee = 0.05f,
                GiveMoneyMinAmount = 100,
                NametagMaxDistance = 1000,
                ShowNametagOnlyOnAiming = true,
                MultiplierRankingKills = 75f,
                MultiplierRankingAssists = 25f,
                MultiplierRankingDamage = 1f,
                GangActionAttackerCanBeMore = true,
                GangActionOwnerCanBeMore = false,
                GitHubRepoOwnerName = "emre1702",
                GitHubRepoRepoName = "TDS-V",
                GangMaxGangActionAttacksPerDay = 5
            });
            return modelBuilder;
        }
    }
}
