using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Database.ModelBuilding.GangEntities
{
    public class GangLevelSettingsConfiguration : IEntityTypeConfiguration<GangLevelSettings>
    {
        public void Configure(EntityTypeBuilder<GangLevelSettings> builder)
        {
            builder.HasKey(e => e.Level);

            builder.Property(e => e.UpgradePrice).IsRequired().HasDefaultValue(int.MaxValue);
            builder.Property(e => e.HousePrice).IsRequired().HasDefaultValue(int.MaxValue);
            builder.Property(e => e.NeededExperience).IsRequired().HasDefaultValue(int.MaxValue);

            builder.Property(e => e.PlayerSlots).IsRequired().HasDefaultValue(byte.MaxValue);
            builder.Property(e => e.RankSlots).IsRequired().HasDefaultValue(byte.MaxValue);
            builder.Property(e => e.VehicleSlots).IsRequired().HasDefaultValue(byte.MaxValue);
            builder.Property(e => e.GangAreaSlots).IsRequired().HasDefaultValue(byte.MaxValue);
            builder.Property(e => e.HouseAreaRadius).IsRequired().HasDefaultValue(30);
        }
    }
}
