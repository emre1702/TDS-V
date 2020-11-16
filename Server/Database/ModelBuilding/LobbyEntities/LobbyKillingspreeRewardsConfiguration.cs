using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyKillingspreeRewardsConfiguration : IEntityTypeConfiguration<LobbyKillingspreeRewards>
    {
        public void Configure(EntityTypeBuilder<LobbyKillingspreeRewards> builder)
        {
            builder.HasKey(e => new { e.LobbyId, e.KillsAmount });

            builder.Property(e => e.KillsAmount).ValueGeneratedNever();

            builder.HasOne(d => d.Lobby)
                .WithMany(p => p.LobbyKillingspreeRewards)
                .HasForeignKey(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
