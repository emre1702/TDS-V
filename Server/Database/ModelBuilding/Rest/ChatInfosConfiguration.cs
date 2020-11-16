using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Database.ModelBuilding.Rest
{
    public class ChatInfosConfiguration : IEntityTypeConfiguration<ChatInfos>
    {
        public void Configure(EntityTypeBuilder<ChatInfos> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
