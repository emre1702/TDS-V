using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Server;

namespace TDS.Server.Database.ModelBuilding.Server
{
    public class ServerTotalStatsConfiguration : IEntityTypeConfiguration<ServerTotalStats>
    {
        public void Configure(EntityTypeBuilder<ServerTotalStats> builder)
        {
            builder.Property(e => e.PlayerPeak).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.ArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CustomArenaRoundsPlayed).IsRequired().HasDefaultValue(0);
        }
    }
}
