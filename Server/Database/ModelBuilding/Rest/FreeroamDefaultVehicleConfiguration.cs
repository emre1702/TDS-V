using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.ModelBuilding.Rest
{
    public class FreeroamDefaultVehicleConfiguration : IEntityTypeConfiguration<FreeroamDefaultVehicle>
    {
        public void Configure(EntityTypeBuilder<FreeroamDefaultVehicle> builder)
        {
            builder.HasKey(e => e.VehicleType);
        }
    }
}
