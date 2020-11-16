using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyFightSettingsConfiguration : IEntityTypeConfiguration<LobbyFightSettings>
    {
        public void Configure(EntityTypeBuilder<LobbyFightSettings> builder)
        {
            builder.HasKey(e => e.LobbyId);

            builder.Property(e => e.LobbyId).ValueGeneratedNever();

            builder.Property(e => e.SpawnAgainAfterDeathMs).HasDefaultValue(400);
            builder.Property(e => e.StartArmor).HasDefaultValue(100);
            builder.Property(e => e.StartHealth).HasDefaultValue(100);
            builder.Property(e => e.AmountLifes).HasDefaultValue(1);

            builder.HasOne(d => d.Lobby)
               .WithOne(p => p.FightSettings)
               .HasForeignKey<LobbyFightSettings>(d => d.LobbyId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
