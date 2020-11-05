using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Admin;

namespace TDS_Server.Database.EntityConfigurations.Admin
{
    internal class AdminLevelsConfiguration : IEntityTypeConfiguration<AdminLevels>
    {
        public void Configure(EntityTypeBuilder<AdminLevels> builder)
        {
            builder.HasKey(e => e.Level);

            builder.Property(e => e.Level).ValueGeneratedNever();
        }
    }
}
