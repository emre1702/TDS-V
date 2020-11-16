using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.ModelBuilding.Rest
{
    public class OfflinemessagesConfiguration : IEntityTypeConfiguration<Offlinemessages>
    {
        public void Configure(EntityTypeBuilder<Offlinemessages> builder)
        {
            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.Message).IsRequired();

            builder.Property(e => e.Timestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.HasOne(d => d.Source)
                .WithMany(p => p.OfflinemessagesSource)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Target)
                .WithMany(p => p.OfflinemessagesTarget)
                .HasForeignKey(d => d.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
