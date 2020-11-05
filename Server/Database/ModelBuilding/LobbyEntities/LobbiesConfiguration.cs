using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbiesConfiguration : IEntityTypeConfiguration<Lobbies>
    {
        public void Configure(EntityTypeBuilder<Lobbies> builder)
        {
            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.AroundSpawnPoint).HasDefaultValueSql("3");

            builder.Property(e => e.CreateTimestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.Property(e => e.DefaultSpawnX).HasDefaultValueSql("0");
            builder.Property(e => e.DefaultSpawnY).HasDefaultValueSql("0");
            builder.Property(e => e.DefaultSpawnZ).HasDefaultValueSql("9000");
            builder.Property(e => e.DefaultSpawnRotation).HasDefaultValueSql("0");
            builder.Property(e => e.IsTemporary);
            builder.Property(e => e.IsOfficial);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Password).IsRequired(false).HasMaxLength(100);

            builder.HasOne(d => d.Owner)
                .WithMany(p => p.Lobbies)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
