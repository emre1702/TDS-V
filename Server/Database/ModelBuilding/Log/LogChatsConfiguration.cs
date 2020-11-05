using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Log;

namespace TDS_Server.Database.ModelBuilding.Log
{
    public class LogChatsConfiguration : IEntityTypeConfiguration<LogChats>
    {
        public void Configure(EntityTypeBuilder<LogChats> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);

            builder.Property(e => e.Message).IsRequired();

            builder.Property(e => e.Timestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");
        }
    }
}
