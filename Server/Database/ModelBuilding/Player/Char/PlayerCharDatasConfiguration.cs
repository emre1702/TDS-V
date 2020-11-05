using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player.Char;

namespace TDS_Server.Database.ModelBuilding.Player.Char
{
    public class PlayerCharDatasConfiguration : IEntityTypeConfiguration<PlayerCharDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerCharDatas> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.OwnsOne(e => e.SyncedData, e =>
            {
                e.Ignore(e => e.GeneralDataSynced);
                e.Ignore(e => e.HeritageDataSynced);
                e.Ignore(e => e.FeaturesDataSynced);
                e.Ignore(e => e.AppearanceDataSynced);
                e.Ignore(e => e.HairAndColorsDataSynced);
            });

            builder.HasOne(e => e.Player)
                .WithOne(p => p.CharDatas)
                .HasForeignKey<PlayerCharDatas>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
