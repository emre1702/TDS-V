using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Server;

namespace TDS.Server.Database.ModelBuilding.Server
{
    public class ServerSettingsConfiguration : IEntityTypeConfiguration<ServerSettings>
    {
        public void Configure(EntityTypeBuilder<ServerSettings> builder)
        {
            builder.Property(e => e.GamemodeName)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(e => e.KillingSpreeMaxSecondsUntilNextKill)
                .IsRequired()
                .HasDefaultValue(18);

            builder.Property(e => e.MapRatingAmountForCheck)
                .IsRequired()
                .HasDefaultValue(10);

            builder.Property(e => e.MinMapRatingForNewMaps)
                .IsRequired()
                .HasDefaultValue(3f);

            builder.Property(e => e.GiveMoneyFee)
                .IsRequired()
                .HasDefaultValue(0.05);

            builder.Property(e => e.GiveMoneyMinAmount)
                .IsRequired()
                .HasDefaultValue(100);

            builder.Property(e => e.NametagMaxDistance)
                .IsRequired()
                .HasDefaultValue(25 * 25);

            builder.Property(e => e.MultiplierRankingKills)
                .IsRequired()
                .HasDefaultValue(75f);

            builder.Property(e => e.MultiplierRankingAssists)
                .IsRequired()
                .HasDefaultValue(25f);

            builder.Property(e => e.MultiplierRankingDamage)
                .IsRequired()
                .HasDefaultValue(1f);

            builder.Property(e => e.ShowNametagOnlyOnAiming)
                .IsRequired();

            builder.Property(e => e.CloseApplicationAfterDays)
                .IsRequired()
                .HasDefaultValue(7);

            builder.Property(e => e.DeleteApplicationAfterDays)
                .IsRequired()
                .HasDefaultValue(14);

            builder.Property(e => e.GangActionPreparationTime)
                .IsRequired()
                .HasDefaultValue(3 * 60);

            builder.Property(e => e.GangActionRoundTime)
                .IsRequired()
                .HasDefaultValue(15 * 60);

            builder.Property(e => e.DeleteRequestsDaysAfterClose)
                .IsRequired()
                .HasDefaultValue(30);

            builder.Property(e => e.DeleteOfflineMessagesAfterDays)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.MinPlayersOnlineForGangAction)
                .IsRequired()
                .HasDefaultValue(3);

            builder.Property(e => e.GangActionAreaAttackCooldownMinutes)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.AmountPlayersAllowedInGangwarTeamBeforeCountCheck)
                .IsRequired()
                .HasDefaultValue(3);

            builder.Property(e => e.GangActionTargetRadius)
                .IsRequired()
                .HasDefaultValue(5d);

            builder.Property(e => e.GangActionTargetWithoutAttackerMaxSeconds)
                .IsRequired()
                .HasDefaultValue(10);

            builder.Property(e => e.ReduceMapsBoughtCounterAfterMinute)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.MapBuyBasePrice)
                .IsRequired()
                .HasDefaultValue(1000);

            builder.Property(e => e.MapBuyCounterMultiplicator)
                .IsRequired()
                .HasDefaultValue(1f);

            builder.Property(e => e.UsernameChangeCost)
                .IsRequired()
                .HasDefaultValue(20000);

            builder.Property(e => e.UsernameChangeCooldownDays)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.AmountWeeklyChallenges)
                .IsRequired()
                .HasDefaultValue(3);

            builder.Property(e => e.ReloadServerBansEveryMinutes)
                .IsRequired()
                .HasDefaultValue(5);

            builder.Property(e => e.AmountCharSlots)
                .IsRequired()
                .HasDefaultValue(3);

            builder.Property(e => e.GitHubRepoOwnerName)
                .IsRequired(false);

            builder.Property(e => e.GitHubRepoRepoName)
                .IsRequired(false);
        }
    }
}
