using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Log;

namespace TDS_Server.Database.ModelBuilding.Log
{
    public class LogAdminsConfiguration : IEntityTypeConfiguration<LogAdmins>
    {
        public void Configure(EntityTypeBuilder<LogAdmins> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);

            builder.Property(e => e.Reason).IsRequired();

            builder.Property(e => e.Timestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");
            builder.Property(e => e.LengthOrEndTime).IsRequired(false);
        }
    }
}
