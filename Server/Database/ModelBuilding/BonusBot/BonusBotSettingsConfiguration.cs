using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Bonusbot;

namespace TDS.Server.Database.ModelBuilding.BonusBot
{
    internal class BonusBotSettingsConfiguration : IEntityTypeConfiguration<BonusbotSettings>
    {
        public void Configure(EntityTypeBuilder<BonusbotSettings> builder)
        {
            builder.Property(e => e.GuildId).IsRequired(false);

            builder.Property(e => e.AdminApplicationsChannelId).IsRequired(false);
            builder.Property(e => e.SupportRequestsChannelId).IsRequired(false);
            builder.Property(e => e.ServerInfosChannelId).IsRequired(false);
            builder.Property(e => e.ActionsInfoChannelId).IsRequired(false);
            builder.Property(e => e.BansInfoChannelId).IsRequired(false);

            builder.Property(e => e.ErrorLogsChannelId).IsRequired(false);

            builder.Ignore(e => e.GrpcDeadline);

            builder.Property(e => e.RefreshServerStatsFrequencySec).IsRequired().HasDefaultValue(60);
        }
    }
}
