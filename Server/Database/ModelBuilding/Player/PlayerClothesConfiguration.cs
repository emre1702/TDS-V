using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
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
