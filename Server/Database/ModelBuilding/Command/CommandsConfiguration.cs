using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Command;

namespace TDS.Server.Database.ModelBuilding.Command
{
    internal class CommandsConfiguration : IEntityTypeConfiguration<Commands>
    {
        public void Configure(EntityTypeBuilder<Commands> builder)
        {
            builder.Property(e => e.Command)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.HasOne(d => d.NeededAdminLevelNavigation)
                .WithMany(p => p.Commands)
                .HasForeignKey(d => d.NeededAdminLevel)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
