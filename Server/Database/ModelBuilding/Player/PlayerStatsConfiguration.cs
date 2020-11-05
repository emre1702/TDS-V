﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
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

            builder.HasOne(d => d.Player)
                .WithOne(p => p.PlayerStats)
                .HasForeignKey<PlayerStats>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
