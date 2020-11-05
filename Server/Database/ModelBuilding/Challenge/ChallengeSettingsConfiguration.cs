using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Challenge;

namespace TDS_Server.Database.ModelBuilding.Challenge
{
    internal class ChallengeSettingsConfiguration : IEntityTypeConfiguration<ChallengeSettings>
    {
        public void Configure(EntityTypeBuilder<ChallengeSettings> builder)
        {
            builder.HasKey(e => new { e.Type, e.Frequency });

            builder.Property(e => e.MinNumber)
                .HasDefaultValue(1);

            builder.Property(e => e.MaxNumber)
                .HasDefaultValue(1);
        }
    }
}
