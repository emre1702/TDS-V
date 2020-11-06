using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerKillInfoSettingsConfiguration : IEntityTypeConfiguration<PlayerKillInfoSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerKillInfoSettings> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.Property(e => e.FontWidth)
                .IsRequired()
                .HasDefaultValue(1.4f);

            builder.Property(e => e.IconWidth)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.Spacing)
                .IsRequired()
                .HasDefaultValue(15);

            builder.HasOne(e => e.Player)
                .WithOne(p => p.KillInfoSettings)
                .HasForeignKey<PlayerKillInfoSettings>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
