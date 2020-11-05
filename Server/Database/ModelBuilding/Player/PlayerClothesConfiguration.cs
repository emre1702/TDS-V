using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerClothesConfiguration : IEntityTypeConfiguration<PlayerClothes>
    {
        public void Configure(EntityTypeBuilder<PlayerClothes> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.HasOne(c => c.Player)
                .WithOne(p => p.PlayerClothes)
                .HasForeignKey<PlayerClothes>(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
