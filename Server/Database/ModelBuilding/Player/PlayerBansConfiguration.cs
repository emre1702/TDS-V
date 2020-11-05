using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerBansConfiguration : IEntityTypeConfiguration<PlayerBans>
    {
        public void Configure(EntityTypeBuilder<PlayerBans> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.LobbyId });

            builder.HasIndex(e => e.IP);
            builder.HasIndex(e => e.SCName);
            builder.HasIndex(e => e.SCId);
            builder.HasIndex(e => e.Serial);

            builder.Property(e => e.EndTimestamp)
                .HasConversion(v => v, v => v == null ? (DateTime?)null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

            builder.Property(e => e.IP).IsRequired(false);

            builder.Property(e => e.Serial).IsRequired(false);

            builder.Property(e => e.SCName).IsRequired(false);

            builder.Property(e => e.SCId).IsRequired(false);

            builder.Property(e => e.Reason).IsRequired();

            builder.Property(e => e.StartTimestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', now())");

            builder.HasOne(d => d.Admin)
                .WithMany(p => p.PlayerBansAdmin)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(d => d.Lobby)
                .WithMany(p => p.PlayerBans)
                .HasForeignKey(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Player)
                .WithMany(p => p.PlayerBansPlayer)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
