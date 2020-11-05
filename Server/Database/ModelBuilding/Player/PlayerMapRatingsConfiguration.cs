using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerMapRatingsConfiguration : IEntityTypeConfiguration<PlayerMapRatings>
    {
        public void Configure(EntityTypeBuilder<PlayerMapRatings> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.MapId });

            builder.HasOne(d => d.Map)
                .WithMany(p => p.PlayerMapRatings)
                .HasForeignKey(d => d.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Player)
                .WithMany(p => p.PlayerMapRatings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
