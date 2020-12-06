using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerHudSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public HudDesign HudDesign { get; set; }

        [JsonProperty("1")]
        public string HudBackgroundColor { get; set; }

        [JsonProperty("2")]
        public string RoundStatsBackgroundColor { get; set; }

        [JsonProperty("3")]
        public string HudFontColor { get; set; }

        [JsonProperty("4")]
        public string RoundStatsFontColor { get; set; }

        [JsonIgnore]
        public virtual PlayerSettings PlayerSettings { get; set; }
    }

    internal class PlayerHudSettingsConfiguration : IEntityTypeConfiguration<PlayerHudSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerHudSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.HudDesign)
                .HasColumnName("HudDesign")
                .HasDefaultValue(HudDesign.BonusV1);

            builder.Property(e => e.HudBackgroundColor)
                .HasColumnName("HudBackgroundColor")
                .IsRequired(false);
            builder.Property(e => e.RoundStatsBackgroundColor)
                .HasColumnName("RoundStatsBackgroundColor")
                .IsRequired(false);
            builder.Property(e => e.HudFontColor)
                .HasColumnName("HudFontColor")
                .IsRequired(false);
            builder.Property(e => e.RoundStatsFontColor)
                .HasColumnName("RoundStatsFontColor")
                .IsRequired(false);
        }
    }
}