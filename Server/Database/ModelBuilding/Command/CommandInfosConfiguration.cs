using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Command;

namespace TDS_Server.Database.ModelBuilding.Command
{
    internal class CommandInfosConfiguration : IEntityTypeConfiguration<CommandInfos>
    {
        public void Configure(EntityTypeBuilder<CommandInfos> builder)
        {
            builder.HasKey(e => new { e.Id, e.Language });

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(d => d.IdNavigation)
                .WithMany(p => p.CommandInfos)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
