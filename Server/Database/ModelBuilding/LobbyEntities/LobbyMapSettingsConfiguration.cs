using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyMapSettingsConfiguration : IEntityTypeConfiguration<LobbyMapSettings>
    {
        public void Configure(EntityTypeBuilder<LobbyMapSettings> builder)
        {
            builder.HasKey(e => e.LobbyId);

            builder.Property(e => e.LobbyId)
               .ValueGeneratedNever();

            builder.Property(e => e.MapLimitTime).HasDefaultValueSql("10");

            builder.HasOne(d => d.Lobby)
                .WithOne(p => p.LobbyMapSettings)
                .HasForeignKey<LobbyMapSettings>(d => d.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
