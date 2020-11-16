using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyMapsConfiguration : IEntityTypeConfiguration<LobbyMaps>
    {
        public void Configure(EntityTypeBuilder<LobbyMaps> builder)
        {
            builder.HasKey(e => new { e.LobbyId, e.MapId });

            builder.HasIndex(e => e.MapId);

            builder.HasOne(d => d.Lobby)
                .WithMany(p => p.LobbyMaps)
                .HasForeignKey(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Map)
                .WithMany(p => p.LobbyMaps)
                .HasForeignKey(d => d.MapId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
