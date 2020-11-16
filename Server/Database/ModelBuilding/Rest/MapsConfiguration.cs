using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.ModelBuilding.Rest
{
    public class MapsConfiguration : IEntityTypeConfiguration<Maps>
    {
        public void Configure(EntityTypeBuilder<Maps> builder)
        {
            builder.HasIndex(e => e.Name)
                    .HasMethod("hash");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.CreateTimestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.Name).IsRequired();

            builder.HasOne(d => d.Creator)
                .WithMany(p => p.Maps)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
