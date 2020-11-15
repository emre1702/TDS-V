using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS_Server.Database.Interfaces;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerCooldownsAndDurationsSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public int BloodscreenCooldownMs { get; set; }

        [JsonProperty("1")]
        public int HudAmmoUpdateCooldownMs { get; set; }

        [JsonProperty("2")]
        public int HudHealthUpdateCooldownMs { get; set; }

        [JsonProperty("3")]
        public int AFKKickAfterSeconds { get; set; }

        [JsonProperty("4")]
        public int AFKKickShowWarningLastSeconds { get; set; }

        [JsonProperty("5")]
        public int ShowFloatingDamageInfoDurationMs { get; set; }
    }

    internal class PlayerCooldownsAndDurationsSettingsConfiguration : IEntityTypeConfiguration<PlayerCooldownsAndDurationsSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerCooldownsAndDurationsSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.BloodscreenCooldownMs)
                .HasColumnName("BloodscreenCooldownMs")
                .HasDefaultValue(150);
            builder.Property(e => e.HudAmmoUpdateCooldownMs)
                .HasColumnName("HudAmmoUpdateCooldownMs")
                .HasDefaultValue(100);
            builder.Property(e => e.HudHealthUpdateCooldownMs)
                .HasColumnName("HudHealthUpdateCooldownMs")
                .HasDefaultValue(100);
            builder.Property(e => e.AFKKickAfterSeconds)
                .HasColumnName("AFKKickAfterSeconds")
                .HasDefaultValue(25);
            builder.Property(e => e.AFKKickShowWarningLastSeconds)
                .HasColumnName("AFKKickShowWarningLastSeconds")
                .HasDefaultValue(10);
            builder.Property(e => e.ShowFloatingDamageInfoDurationMs)
                .HasColumnName("ShowFloatingDamageInfoDurationMs")
                .HasDefaultValue(1000);
        }
    }
}
