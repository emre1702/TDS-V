using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangRankPermissionsConfiguration : IEntityTypeConfiguration<GangRankPermissions>
    {
        public void Configure(EntityTypeBuilder<GangRankPermissions> builder)
        {
            builder.HasKey(e => e.GangId);

            builder.Property(e => e.GangId);

            builder.HasOne(e => e.Gang)
                .WithOne(g => g.RankPermissions)
                .HasForeignKey<GangRankPermissions>(e => e.GangId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
