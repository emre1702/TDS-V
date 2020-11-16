using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerVoiceSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public bool Voice3D { get; set; }

        [JsonProperty("1")]
        public bool VoiceAutoVolume { get; set; }

        [JsonProperty("2")]
        public float VoiceVolume { get; set; }
    }

    internal class PlayerVoiceSettingsConfiguration : IEntityTypeConfiguration<PlayerVoiceSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerVoiceSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.Voice3D)
                .HasColumnName("Voice3D");
            builder.Property(e => e.VoiceAutoVolume)
                .HasColumnName("VoiceAutoVolume");
            builder.Property(e => e.VoiceVolume)
                .HasColumnName("VoiceVolume")
                .HasDefaultValue(6.0);
        }
    }
}
