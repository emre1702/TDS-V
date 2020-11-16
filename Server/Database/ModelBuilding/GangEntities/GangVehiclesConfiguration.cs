using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Database.ModelBuilding.GangEntities
{
    public class GangVehiclesConfiguration : IEntityTypeConfiguration<GangVehicles>
    {
        public void Configure(EntityTypeBuilder<GangVehicles> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Gang)
                .WithMany(g => g.Vehicles)
                .HasForeignKey(e => e.GangId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
