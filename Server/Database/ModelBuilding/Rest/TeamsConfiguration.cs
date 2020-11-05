using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.ModelBuilding.Rest
{
    public class TeamsConfiguration : IEntityTypeConfiguration<Teams>
    {
        public void Configure(EntityTypeBuilder<Teams> builder)
        {
            builder.Property(e => e.Id)
                    .UseIdentityAlwaysColumn();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("Spectator");

            builder.Property(e => e.SkinHash)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.BlipColor)
                .HasDefaultValue(4);

            builder.HasOne(d => d.LobbyNavigation)
                .WithMany(p => p.Teams)
                .HasForeignKey(d => d.Lobby)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
