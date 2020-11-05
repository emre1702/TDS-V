using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.ModelBuilding.Rest
{
    public class FreeroamDefaultVehicleConfiguration : IEntityTypeConfiguration<FreeroamDefaultVehicle>
    {
        public void Configure(EntityTypeBuilder<FreeroamDefaultVehicle> builder)
        {
            builder.HasKey(e => e.VehicleType);
        }
    }
}
