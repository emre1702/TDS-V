using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangRanksConfiguration : IEntityTypeConfiguration<GangRanks>
    {
        public void Configure(EntityTypeBuilder<GangRanks> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Rank);

            builder.HasOne(e => e.Gang)
                .WithMany(g => g.Ranks)
                .HasForeignKey(e => e.GangId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
