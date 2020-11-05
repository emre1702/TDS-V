using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
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
