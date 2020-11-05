using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Challenge;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.SeedData.Challenge
{
    public static class ChallengeSettingsSeeds
    {
        public static ModelBuilder HasChallengeSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallengeSettings>().HasData(
                new ChallengeSettings { Type = ChallengeType.Assists, Frequency = ChallengeFrequency.Weekly, MinNumber = 50, MaxNumber = 100 },
                new ChallengeSettings { Type = ChallengeType.BeHelpfulEnough, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.BombDefuse, Frequency = ChallengeFrequency.Weekly, MinNumber = 5, MaxNumber = 10 },
                new ChallengeSettings { Type = ChallengeType.BombPlant, Frequency = ChallengeFrequency.Weekly, MinNumber = 5, MaxNumber = 10 },
                new ChallengeSettings { Type = ChallengeType.BuyMaps, Frequency = ChallengeFrequency.Forever, MinNumber = 500, MaxNumber = 500 },
                new ChallengeSettings { Type = ChallengeType.ChangeSettings, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.CreatorOfAcceptedMap, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.Damage, Frequency = ChallengeFrequency.Weekly, MinNumber = 20000, MaxNumber = 100000 },
                new ChallengeSettings { Type = ChallengeType.JoinDiscordServer, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.Kills, Frequency = ChallengeFrequency.Weekly, MinNumber = 75, MaxNumber = 150 },
                new ChallengeSettings { Type = ChallengeType.Killstreak, Frequency = ChallengeFrequency.Weekly, MinNumber = 3, MaxNumber = 7 },
                new ChallengeSettings { Type = ChallengeType.PlayTime, Frequency = ChallengeFrequency.Weekly, MinNumber = 300, MaxNumber = 1500 },
                new ChallengeSettings { Type = ChallengeType.ReadTheFAQ, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.ReadTheRules, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 },
                new ChallengeSettings { Type = ChallengeType.ReviewMaps, Frequency = ChallengeFrequency.Forever, MinNumber = 10, MaxNumber = 10 },
                new ChallengeSettings { Type = ChallengeType.RoundPlayed, Frequency = ChallengeFrequency.Weekly, MinNumber = 50, MaxNumber = 100 },
                new ChallengeSettings { Type = ChallengeType.WriteHelpfulIssue, Frequency = ChallengeFrequency.Forever, MinNumber = 1, MaxNumber = 1 }
            );

            return modelBuilder;
        }
    }
}
