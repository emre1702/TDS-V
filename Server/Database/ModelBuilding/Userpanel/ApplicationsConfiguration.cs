using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Userpanel;

namespace TDS.Server.Database.ModelBuilding.Userpanel
{
    public class ApplicationsConfiguration : IEntityTypeConfiguration<Applications>
    {
        public void Configure(EntityTypeBuilder<Applications> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.CreateTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.HasOne(app => app.Player)
                .WithOne(player => player.Application)
                .HasForeignKey<Applications>(app => app.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
