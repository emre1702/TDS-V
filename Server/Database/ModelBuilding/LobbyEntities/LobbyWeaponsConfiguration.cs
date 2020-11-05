using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.ModelBuilding.LobbyEntities
{
    public class LobbyWeaponsConfiguration : IEntityTypeConfiguration<LobbyWeapons>
    {
        public void Configure(EntityTypeBuilder<LobbyWeapons> builder)
        {
            builder.HasKey(e => new { e.Hash, e.Lobby });

            builder.Property(e => e.Hash).ValueGeneratedNever();

            builder.HasOne(d => d.HashNavigation)
                .WithMany(p => p.LobbyWeapons)
                .HasForeignKey(d => d.Hash)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.LobbyNavigation)
                .WithMany(p => p.LobbyWeapons)
                .HasForeignKey(d => d.Lobby)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
