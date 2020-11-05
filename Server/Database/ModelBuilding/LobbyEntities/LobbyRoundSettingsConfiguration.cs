using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyRoundSettingsConfiguration : IEntityTypeConfiguration<LobbyRoundSettings>
    {
        public void Configure(EntityTypeBuilder<LobbyRoundSettings> builder)
        {
            builder.HasKey(e => e.LobbyId);

            builder.Property(e => e.LobbyId)
                .ValueGeneratedNever();

            builder.Property(e => e.BombDefuseTimeMs).HasDefaultValueSql("8000");

            builder.Property(e => e.BombDetonateTimeMs).HasDefaultValueSql("45000");

            builder.Property(e => e.BombPlantTimeMs).HasDefaultValueSql("3000");

            builder.Property(e => e.CountdownTime).HasDefaultValueSql("5");

            builder.Property(e => e.RoundTime).HasDefaultValueSql("240");

            builder.HasOne(d => d.Lobby)
                .WithOne(p => p.LobbyRoundSettings)
                .HasForeignKey<LobbyRoundSettings>(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
