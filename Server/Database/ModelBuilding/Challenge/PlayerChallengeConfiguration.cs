using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Challenge;

namespace TDS.Server.Database.ModelBuilding.Challenge
{
    internal class PlayerChallengeConfiguration : IEntityTypeConfiguration<PlayerChallenges>
    {
        public void Configure(EntityTypeBuilder<PlayerChallenges> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Challenge, e.Frequency });

            builder.Property(e => e.Amount).HasDefaultValue(1);

            builder.Property(e => e.CurrentAmount).HasDefaultValue(0);

            builder.HasOne(c => c.Player)
                .WithMany(p => p.Challenges)
                .HasForeignKey(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
