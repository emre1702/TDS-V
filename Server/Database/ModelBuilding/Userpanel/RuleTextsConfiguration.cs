using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Userpanel;

namespace TDS.Server.Database.ModelBuilding.Userpanel
{
    public class RuleTextsConfiguration : IEntityTypeConfiguration<RuleTexts>
    {
        public void Configure(EntityTypeBuilder<RuleTexts> builder)
        {
            builder.HasKey(e => new { e.RuleId, e.Language });

            builder.HasOne(d => d.Rule)
                .WithMany(p => p.RuleTexts)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
