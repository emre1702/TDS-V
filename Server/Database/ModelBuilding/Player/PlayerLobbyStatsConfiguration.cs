using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerLobbyStatsConfiguration : IEntityTypeConfiguration<PlayerLobbyStats>
    {
        public void Configure(EntityTypeBuilder<PlayerLobbyStats> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.LobbyId });

            builder.HasOne(d => d.Lobby)
                .WithMany(p => p.PlayerLobbyStats)
                .HasForeignKey(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Player)
                .WithMany(p => p.PlayerLobbyStats)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
