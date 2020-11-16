using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangHousesConfiguration : IEntityTypeConfiguration<GangHouses>
    {
        public void Configure(EntityTypeBuilder<GangHouses> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.LastBought)
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            builder.Property(e => e.Created)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.HasOne(e => e.Creator)
                .WithMany(p => p.CreatedHouses)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
