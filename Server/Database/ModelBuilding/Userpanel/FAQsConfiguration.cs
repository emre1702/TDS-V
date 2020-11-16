using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Userpanel;

namespace TDS.Server.Database.ModelBuilding.Userpanel
{
    public class FAQsConfiguration : IEntityTypeConfiguration<FAQs>
    {
        public void Configure(EntityTypeBuilder<FAQs> builder)
        {
            builder.HasKey(e => new { e.Id, e.Language });
        }
    }
}
