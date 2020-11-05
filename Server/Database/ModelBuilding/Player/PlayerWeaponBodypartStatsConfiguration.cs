using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerWeaponBodypartStatsConfiguration : IEntityTypeConfiguration<PlayerWeaponBodypartStats>
    {
        public void Configure(EntityTypeBuilder<PlayerWeaponBodypartStats> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.WeaponHash, e.BodyPart });

            builder.Property(e => e.AmountHits).HasDefaultValue(0);
            builder.Property(e => e.AmountOfficialHits).HasDefaultValue(0);
            builder.Property(e => e.DealtDamage).HasDefaultValue(0);
            builder.Property(e => e.DealtOfficialDamage).HasDefaultValue(0);
            builder.Property(e => e.Kills).HasDefaultValue(0);
            builder.Property(e => e.OfficialKills).HasDefaultValue(0);

            builder.HasOne(e => e.Player)
                .WithMany(p => p.WeaponBodypartStats)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Weapon)
                .WithMany(p => p.PlayerWeaponBodypartStats)
                .HasForeignKey(e => e.WeaponHash)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
