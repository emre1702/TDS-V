using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangwarAreasConfiguration : IEntityTypeConfiguration<GangwarAreas>
    {
        public void Configure(EntityTypeBuilder<GangwarAreas> builder)
        {
            builder.HasKey(e => e.MapId);

            builder.Property(e => e.LastAttacked)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("'2019-1-1'::timestamp");

            builder.Property(e => e.AttackCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.DefendCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(g => g.Map)
                .WithOne(m => m.GangwarArea)
                .HasForeignKey<GangwarAreas>(g => g.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.OwnerGang)
                .WithMany(m => m.GangwarAreas)
                .HasForeignKey(g => g.OwnerGangId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
