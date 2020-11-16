using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Userpanel;

namespace TDS.Server.Database.ModelBuilding.Userpanel
{
    public class ApplicationAnswersConfiguration : IEntityTypeConfiguration<ApplicationAnswers>
    {
        public void Configure(EntityTypeBuilder<ApplicationAnswers> builder)
        {
            builder.HasKey(e => new { e.ApplicationId, e.QuestionId });

            builder.Property(e => e.ApplicationId);
            builder.Property(e => e.QuestionId);

            builder.HasOne(answer => answer.Application)
                .WithMany(app => app.Answers)
                .HasForeignKey(answer => answer.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(answer => answer.Question)
                .WithMany(app => app.Answers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
