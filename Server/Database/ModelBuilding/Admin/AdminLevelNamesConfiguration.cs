using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Admin;

namespace TDS.Server.Database.EntityConfigurations.Admin
{
    internal class AdminLevelNamesConfiguration : IEntityTypeConfiguration<AdminLevelNames>
    {
        public void Configure(EntityTypeBuilder<AdminLevelNames> builder)
        {
            builder.HasKey(e => new { e.Level, e.Language });

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(d => d.LevelNavigation)
                .WithMany(p => p.AdminLevelNames)
                .HasForeignKey(d => d.Level)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
