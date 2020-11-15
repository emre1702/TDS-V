using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Interfaces;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerThemeSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public bool UseDarkTheme { get; set; }

        [JsonProperty("1")]
        public string ThemeMainColor { get; set; }

        [JsonProperty("2")]
        public string ThemeSecondaryColor { get; set; }

        [JsonProperty("3")]
        public string ThemeWarnColor { get; set; }

        [JsonProperty("4")]
        public string ThemeBackgroundDarkColor { get; set; }

        [JsonProperty("5")]
        public string ThemeBackgroundLightColor { get; set; }

        [JsonProperty("6")]
        public int ToolbarDesign { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }
    }
     
    public class PlayerThemeSettingsConfiguration : IEntityTypeConfiguration<PlayerThemeSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerThemeSettings> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.Property(e => e.ThemeMainColor).HasDefaultValue("rgba(0,0,77,1)");
            builder.Property(e => e.ThemeSecondaryColor).HasDefaultValue("rgba(255,152,0,1)");
            builder.Property(e => e.ThemeWarnColor).HasDefaultValue("rgba(244,67,54,1)");
            builder.Property(e => e.ThemeBackgroundDarkColor).HasDefaultValue("linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)");
            builder.Property(e => e.ThemeBackgroundLightColor).HasDefaultValue("rgba(250, 250, 250, 0.87)");
            builder.Property(e => e.ToolbarDesign).HasDefaultValue(1);

            builder.HasOne(e => e.Player)
                .WithOne(p => p.ThemeSettings)
                .HasForeignKey<PlayerThemeSettings>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}