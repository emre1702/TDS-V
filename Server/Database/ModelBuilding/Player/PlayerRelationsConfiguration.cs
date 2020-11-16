using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerRelationsConfiguration : IEntityTypeConfiguration<PlayerRelations>
    {
        public void Configure(EntityTypeBuilder<PlayerRelations> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.TargetId });

            builder.Property(e => e.Relation).IsRequired();

            builder.HasOne(d => d.Player)
                .WithMany(p => p.PlayerRelationsPlayer)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Target)
                .WithMany(p => p.PlayerRelationsTarget)
                .HasForeignKey(d => d.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
