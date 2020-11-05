using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.ModelBuilding.Rest
{
    public class AnnouncementsConfiguration : IEntityTypeConfiguration<Announcements>
    {
        public void Configure(EntityTypeBuilder<Announcements> builder)
        {
            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.Created)
                .IsRequired()
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.Text)
                .IsRequired();
        }
    }
}
