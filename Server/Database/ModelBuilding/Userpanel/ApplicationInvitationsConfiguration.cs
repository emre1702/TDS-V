using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Userpanel;

namespace TDS_Server.Database.ModelBuilding.Userpanel
{
    public class ApplicationInvitationsConfiguration : IEntityTypeConfiguration<ApplicationInvitations>
    {
        public void Configure(EntityTypeBuilder<ApplicationInvitations> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.HasOne(invitation => invitation.Application)
                .WithMany(app => app.Invitations)
                .HasForeignKey(answer => answer.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(invitation => invitation.Admin)
               .WithMany(app => app.ApplicationInvitations)
               .HasForeignKey(answer => answer.AdminId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
