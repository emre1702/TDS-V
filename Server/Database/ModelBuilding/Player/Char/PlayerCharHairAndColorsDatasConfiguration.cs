﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player.Char;

namespace TDS_Server.Database.ModelBuilding.Player.Char
{
    public class PlayerCharHairAndColorsDatasConfiguration : IEntityTypeConfiguration<PlayerCharHairAndColorsDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerCharHairAndColorsDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.OwnsOne(e => e.SyncedData);

            builder.HasOne(e => e.CharDatas)
                .WithMany(c => c.HairAndColorsData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}