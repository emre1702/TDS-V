using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.ModelBuilding.Player
{
    public class PlayerSettingsConfiguration : IEntityTypeConfiguration<PlayerSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerSettings> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.HasIndex(e => e.DiscordUserId).IsUnique();

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.Property(e => e.AllowDataTransfer);
            builder.Property(e => e.Bloodscreen);
            builder.Property(e => e.FloatingDamageInfo);
            builder.Property(e => e.Hitsound);
            builder.Property(e => e.Language).HasDefaultValue(Language.English);
            builder.Property(e => e.VoiceVolume).HasDefaultValue(6.0);
            builder.Property(e => e.MapBorderColor).HasDefaultValue("rgba(150,0,0,0.35)");
            builder.Property(e => e.ShowConfettiAtRanking);
            builder.Property(e => e.DiscordUserId).IsRequired(false);
            builder.Property(e => e.Timezone)
                .HasDefaultValue("UTC");
            builder.Property(e => e.DateTimeFormat)
                .HasDefaultValue("yyyy'-'MM'-'dd HH':'mm':'ss");
            builder.Property(e => e.BloodscreenCooldownMs)
                .HasDefaultValue(150);
            builder.Property(e => e.HudAmmoUpdateCooldownMs)
                .HasDefaultValue(100);
            builder.Property(e => e.HudHealthUpdateCooldownMs)
                .HasDefaultValue(100);
            builder.Property(e => e.AFKKickAfterSeconds)
                .HasDefaultValue(25);
            builder.Property(e => e.AFKKickShowWarningLastSeconds)
                .HasDefaultValue(10);
            builder.Property(e => e.ShowFloatingDamageInfoDurationMs)
                .HasDefaultValue(1000);
            builder.Property(e => e.NametagDeadColor)
                .HasDefaultValue("rgba(0, 0, 0, 1)")
                .IsRequired(false);
            builder.Property(e => e.NametagHealthEmptyColor)
                .HasDefaultValue("rgba(50, 0, 0, 1)");
            builder.Property(e => e.NametagHealthFullColor)
                .HasDefaultValue("rgba(0, 255, 0, 1)");
            builder.Property(e => e.NametagArmorEmptyColor)
                .IsRequired(false);
            builder.Property(e => e.NametagArmorFullColor)
                .HasDefaultValue("rgba(255, 255, 255, 1)");
            builder.Property(e => e.ChatWidth).HasDefaultValue(30);
            builder.Property(e => e.ChatMaxHeight).HasDefaultValue(35);
            builder.Property(e => e.ChatFontSize).HasDefaultValue(1.4);
            builder.Property(e => e.ChatInfoFontSize).HasDefaultValue(1f);
            builder.Property(e => e.ChatInfoMoveTimeMs).HasDefaultValue(15000);
            builder.Property(e => e.ScoreboardPlaytimeUnit).HasDefaultValue(TimeSpanUnitsOfTime.HourMinute);

            builder.HasOne(d => d.Player)
                .WithOne(p => p.PlayerSettings)
                .HasForeignKey<PlayerSettings>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
