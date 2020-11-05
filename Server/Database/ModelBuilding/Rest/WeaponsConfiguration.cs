using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.ModelBuilding.Rest
{
    public class WeaponsConfiguration : IEntityTypeConfiguration<Weapons>
    {
        public void Configure(EntityTypeBuilder<Weapons> builder)
        {
            builder.HasKey(e => e.Hash);

            builder.Property(e => e.Hash).IsRequired();

            builder.Property(e => e.ClipSize).HasDefaultValue(0);
            builder.Property(e => e.MinHeadShotDistance).HasDefaultValue(0);
            builder.Property(e => e.MaxHeadShotDistance).HasDefaultValue(0);
            builder.Property(e => e.HeadShotDamageModifier).HasDefaultValue(0);
            builder.Property(e => e.Damage).HasDefaultValue(0);
            builder.Property(e => e.HitLimbsDamageModifier).HasDefaultValue(0);
            builder.Property(e => e.ReloadTime).HasDefaultValue(0);
            builder.Property(e => e.TimeBetweenShots).HasDefaultValue(0);
            builder.Property(e => e.Range).HasDefaultValue(0);

            builder.Property(e => e.DamageExpMult).HasDefaultValue(0);
            builder.Property(e => e.HeadshotsExpMult).HasDefaultValue(0);
            builder.Property(e => e.HitsExpMult).HasDefaultValue(0);
            builder.Property(e => e.KillsExpMult).HasDefaultValue(0);
            builder.Property(e => e.ShotsExpMult).HasDefaultValue(0);
        }
    }
}
