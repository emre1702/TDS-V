using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Log;

namespace TDS.Server.Database.ModelBuilding.Log
{
    public class LogErrorsConfiguration : IEntityTypeConfiguration<LogErrors>
    {
        public void Configure(EntityTypeBuilder<LogErrors> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);

            builder.Property(e => e.ExceptionType).IsRequired(false);

            builder.Property(e => e.Timestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");
        }
    }
}
