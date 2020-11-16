using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangStatsConfiguration : IEntityTypeConfiguration<GangStats>
    {
        public void Configure(EntityTypeBuilder<GangStats> builder)
        {
            builder.HasKey(e => e.GangId);

            builder.Property(e => e.Money).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.Experience).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.AmountAttacks).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.AmountDefends).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.AmountAttacksWon).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.AmountDefendsWon).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.AmountMembersSoFar).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.PeakGangwarAreasOwned).IsRequired(true).HasDefaultValue(0);
            builder.Property(e => e.TotalMoneySoFar).IsRequired(true).HasDefaultValue(0);

            builder.HasOne(e => e.Gang)
                .WithOne(g => g.Stats)
                .HasForeignKey<GangStats>(e => e.GangId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
