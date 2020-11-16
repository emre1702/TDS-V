using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerTotalStatsConfiguration : IEntityTypeConfiguration<PlayerTotalStats>
    {
        public void Configure(EntityTypeBuilder<PlayerTotalStats> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.HasOne(d => d.Player)
               .WithOne(p => p.PlayerTotalStats)
               .HasForeignKey<PlayerTotalStats>(d => d.PlayerId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
