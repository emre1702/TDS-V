using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerFightEffectSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public bool Bloodscreen { get; set; }

        [JsonProperty("1")]
        public bool Hitsound { get; set; }

        [JsonProperty("2")]
        public bool FloatingDamageInfo { get; set; }
    }

    internal class PlayerFightEffectSettingsConfiguration : IEntityTypeConfiguration<PlayerFightEffectSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerFightEffectSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.Bloodscreen).HasColumnName("Bloodscreen");
            builder.Property(e => e.Hitsound).HasColumnName("Hitsound");
            builder.Property(e => e.FloatingDamageInfo).HasColumnName("FloatingDamageInfo");
        }
    }
}
