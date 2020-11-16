using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayersConfiguration : IEntityTypeConfiguration<Players>
    {
        public void Configure(EntityTypeBuilder<Players> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(e => e.Donation).HasDefaultValue(0);

            builder.Property(e => e.AdminLvl).HasDefaultValue(0);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.AdminLeaderId)
                .IsRequired(false);

            builder.Property(e => e.RegisterTimestamp)
                .HasDefaultValueSql("timezone('utc', now())")
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(e => e.SCName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.SCId)
                .IsRequired();

            builder.HasOne(d => d.AdminLvlNavigation)
                .WithMany(p => p.Players)
                .HasForeignKey(d => d.AdminLvl)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.AdminLeader)
                .WithMany(p => p.AdminMembers)
                .HasForeignKey(p => p.AdminLeaderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
