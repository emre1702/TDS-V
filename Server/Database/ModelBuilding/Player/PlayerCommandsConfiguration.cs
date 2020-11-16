using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerCommandsConfiguration : IEntityTypeConfiguration<PlayerCommands>
    {
        public void Configure(EntityTypeBuilder<PlayerCommands> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.PlayerId);

            builder.Property(e => e.CommandText).HasMaxLength(100);

            builder.HasOne(c => c.Player)
                .WithMany(p => p.Commands)
                .HasForeignKey(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Command)
                .WithMany(p => p.PlayerCommands)
                .HasForeignKey(c => c.CommandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
