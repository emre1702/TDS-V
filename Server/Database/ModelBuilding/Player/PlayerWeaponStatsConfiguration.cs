using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.ModelBuilding.Player
{
    public class PlayerWeaponStatsConfiguration : IEntityTypeConfiguration<PlayerWeaponStats>
    {
        public void Configure(EntityTypeBuilder<PlayerWeaponStats> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.WeaponHash });

            builder.Property(e => e.AmountHeadshots).HasDefaultValue(0);
            builder.Property(e => e.AmountOfficialHeadshots).HasDefaultValue(0);
            builder.Property(e => e.AmountHits).HasDefaultValue(0);
            builder.Property(e => e.AmountOfficialHits).HasDefaultValue(0);
            builder.Property(e => e.AmountShots).HasDefaultValue(0);
            builder.Property(e => e.AmountOfficialShots).HasDefaultValue(0);
            builder.Property(e => e.DealtDamage).HasDefaultValue(0);
            builder.Property(e => e.DealtOfficialDamage).HasDefaultValue(0);
            builder.Property(e => e.Kills).HasDefaultValue(0);
            builder.Property(e => e.OfficialKills).HasDefaultValue(0);

            builder.HasOne(e => e.Player)
                .WithMany(p => p.WeaponStats)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Weapon)
                .WithMany(p => p.PlayerWeaponStats)
                .HasForeignKey(e => e.WeaponHash)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
