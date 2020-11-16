using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Command;

namespace TDS.Server.Database.ModelBuilding.Command
{
    internal class CommandAliasConfiguration : IEntityTypeConfiguration<CommandAlias>
    {
        public void Configure(EntityTypeBuilder<CommandAlias> builder)
        {
            builder.HasKey(e => new { e.Alias, e.Command });

            builder.Property(e => e.Alias).HasMaxLength(100);

            builder.HasOne(d => d.CommandNavigation)
                .WithMany(p => p.CommandAlias)
                .HasForeignKey(d => d.Command)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
