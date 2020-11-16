using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TDS.Server.Database.Entity.Server;

namespace TDS.Server.Database.ModelBuilding.Server
{
    public class ServerDailyStatsConfiguration : IEntityTypeConfiguration<ServerDailyStats>
    {
        public void Configure(EntityTypeBuilder<ServerDailyStats> builder)
        {
            builder.HasKey(e => e.Date);

            builder.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("date")
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("timezone('utc', CURRENT_DATE)");
            builder.Property(e => e.PlayerPeak).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.ArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CustomArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.AmountLogins).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.AmountRegistrations).IsRequired().HasDefaultValue(0);
        }
    }
}
