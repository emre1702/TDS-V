using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.ModelBuilding.Userpanel
{
    public class SupportRequestsConfiguration : IEntityTypeConfiguration<SupportRequests>
    {
        public void Configure(EntityTypeBuilder<SupportRequests> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(100);

            builder.Property(e => e.CreateTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");

            builder.Property(e => e.CloseTime)
               .HasConversion(v => v, v => v == null ? (DateTime?)null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

            builder.HasOne(e => e.Author)
                .WithOne(a => a.SupportRequests)
                .HasForeignKey<SupportRequests>(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
