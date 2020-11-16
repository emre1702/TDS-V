using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Player.Char;

namespace TDS.Server.Database.ModelBuilding.Player.Char
{
    public class PlayerCharHeritageDatasConfiguration : IEntityTypeConfiguration<PlayerCharHeritageDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerCharHeritageDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.OwnsOne(e => e.SyncedData);

            builder.HasOne(e => e.CharDatas)
                .WithMany(c => c.HeritageData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
