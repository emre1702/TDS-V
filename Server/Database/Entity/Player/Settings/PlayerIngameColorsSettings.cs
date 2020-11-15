using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS_Server.Database.Interfaces;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerIngameColorsSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public string MapBorderColor { get; set; }

        [JsonProperty("1")]
        public string NametagDeadColor { get; set; }

        [JsonProperty("2")]
        public string NametagHealthEmptyColor { get; set; }

        [JsonProperty("3")]
        public string NametagHealthFullColor { get; set; }

        [JsonProperty("4")]
        public string NametagArmorEmptyColor { get; set; }

        [JsonProperty("5")]
        public string NametagArmorFullColor { get; set; } 
    }

    internal class PlayerIngameColorsSettingsConfiguration : IEntityTypeConfiguration<PlayerIngameColorsSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerIngameColorsSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.MapBorderColor)
                .HasColumnName("MapBorderColor")
                .HasDefaultValue("rgba(150,0,0,0.35)");
            builder.Property(e => e.NametagDeadColor)
                .HasColumnName("NametagDeadColor")
                .HasDefaultValue("rgba(0, 0, 0, 1)")
                .IsRequired(false);
            builder.Property(e => e.NametagHealthEmptyColor)
                .HasColumnName("NametagHealthEmptyColor")
                .HasDefaultValue("rgba(50, 0, 0, 1)");
            builder.Property(e => e.NametagHealthFullColor)
                .HasColumnName("NametagHealthFullColor")
                .HasDefaultValue("rgba(0, 255, 0, 1)");
            builder.Property(e => e.NametagArmorEmptyColor)
                .HasColumnName("NametagArmorEmptyColor")
                .IsRequired(false);
            builder.Property(e => e.NametagArmorFullColor)
                .HasColumnName("NametagArmorFullColor")
                .HasDefaultValue("rgba(255, 255, 255, 1)");
        }
    }
}
