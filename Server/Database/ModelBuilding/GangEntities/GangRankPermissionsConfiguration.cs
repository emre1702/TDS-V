using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Database.ModelBuilding.GangEntities
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
