using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerStatsConfiguration : IEntityTypeConfiguration<PlayerStats>
    {
        public void Configure(EntityTypeBuilder<PlayerStats> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.Property(e => e.MapsBoughtCounter)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(e => e.LastLoginTimestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.LastMapsBoughtCounterReduce)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.LastFreeUsernameChange)
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null)
                .IsRequired(false);

            builder.Property(e => e.AmountLowPriorityIssues).HasDefaultValue(0);
            builder.Property(e => e.AmountMediumPriorityIssues).HasDefaultValue(0);
            builder.Property(e => e.AmountHighPriorityIssues).HasDefaultValue(0);
            builder.Property(e => e.AmountUrgentPriorityIssues).HasDefaultValue(0);

            builder.HasOne(d => d.Player)
                .WithOne(p => p.PlayerStats)
                .HasForeignKey<PlayerStats>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
