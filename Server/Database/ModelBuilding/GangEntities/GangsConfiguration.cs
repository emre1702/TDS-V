using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangsConfiguration : IEntityTypeConfiguration<Gangs>
    {
        public void Configure(EntityTypeBuilder<Gangs> builder)
        {
            builder.Property(e => e.Id)
                    .UseIdentityAlwaysColumn();

            builder.Property(e => e.NameShort)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.BlipColor)
                .HasDefaultValue(0);

            builder.Property(e => e.HouseId).IsRequired(false);

            builder.Property(e => e.OwnerId).IsRequired(false);

            // Not required so we can set Owner to null when Owner gets deleted
            builder.Property(e => e.OwnerId)
                .IsRequired(false);

            builder.Property(e => e.CreateTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.BlipColor).IsRequired(true).HasDefaultValue(1);

            builder.HasOne(d => d.Team)
                .WithMany(p => p.Gangs)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Owner)
                .WithOne(o => o.OwnedGang)
                .HasForeignKey<Gangs>(o => o.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.House)
               .WithOne(g => g.OwnerGang)
               .HasForeignKey<Gangs>(e => e.HouseId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
