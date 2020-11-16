using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyRewardsConfiguration : IEntityTypeConfiguration<LobbyRewards>
    {
        public void Configure(EntityTypeBuilder<LobbyRewards> builder)
        {
            builder.HasKey(e => e.LobbyId);

            builder.Property(e => e.LobbyId)
                .ValueGeneratedNever();

            builder.HasOne(d => d.Lobby)
                .WithOne(p => p.LobbyRewards)
                .HasForeignKey<LobbyRewards>(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
