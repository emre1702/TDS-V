using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.ModelBuilding.Userpanel
{
    public class RulesConfiguration : IEntityTypeConfiguration<Rules>
    {
        public void Configure(EntityTypeBuilder<Rules> builder)
        {

        }
    }
}
