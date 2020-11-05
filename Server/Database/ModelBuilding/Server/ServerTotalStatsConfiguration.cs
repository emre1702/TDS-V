using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Server;

namespace TDS_Server.Database.ModelBuilding.Server
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
