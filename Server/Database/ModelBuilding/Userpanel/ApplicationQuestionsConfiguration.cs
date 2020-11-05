using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.ModelBuilding.Userpanel
{
    public class ApplicationQuestionsConfiguration : IEntityTypeConfiguration<ApplicationQuestions>
    {
        public void Configure(EntityTypeBuilder<ApplicationQuestions> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.HasOne(question => question.Admin)
                .WithMany(admin => admin.ApplicationQuestions)
                .HasForeignKey(answer => answer.AdminId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
