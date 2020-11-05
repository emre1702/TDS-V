using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyArmsRaceWeaponsConfiguration : IEntityTypeConfiguration<LobbyArmsRaceWeapons>
    {
        public void Configure(EntityTypeBuilder<LobbyArmsRaceWeapons> builder)
        {
            builder.HasKey(e => new { e.LobbyId, e.AtKill });

            builder.Property(e => e.WeaponHash).IsRequired(false);

            builder.HasOne(e => e.Lobby)
                .WithMany(l => l.ArmsRaceWeapons)
                .HasForeignKey(e => e.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Weapon)
                .WithMany(w => w.ArmsRaceWeapons)
                .HasForeignKey(e => e.WeaponHash)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
