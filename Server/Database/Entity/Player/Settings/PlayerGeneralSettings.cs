using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.EntityConfigurations;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Player.Settings
{
    public class PlayerGeneralSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public Language Language { get; set; }

        [JsonProperty("1")]
        public bool AllowDataTransfer { get; set; }

        [JsonProperty("2")]
        public bool ShowConfettiAtRanking { get; set; }

        [JsonProperty("3")]
        public string Timezone { get; set; }

        [JsonProperty("4")]
        public string DateTimeFormat { get; set; }

        [JsonProperty("5")]
        public ulong? DiscordUserId { get; set; }

        [JsonProperty("6")]
        public bool CheckAFK { get; set; }

        [JsonProperty("7")]
        public bool WindowsNotifications { get; set; }

        public virtual PlayerSettings PlayerSettings { get; set; }
    }

    internal class PlayerGeneralSettingsConfiguration : IEntityTypeConfiguration<PlayerGeneralSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerGeneralSettings> builder)
        {
            builder.ToTable("PlayerSettings");

            builder.HasKey(e => e.PlayerId);
            builder.HasIndex(e => e.DiscordUserId).IsUnique();

            builder.Property(e => e.Language)
                .HasColumnName("Language")
                .HasDefaultValue(Language.English);
            builder.Property(e => e.AllowDataTransfer)
                .HasColumnName("AllowDataTransfer");
            builder.Property(e => e.ShowConfettiAtRanking)
                .HasColumnName("ShowConfettiAtRanking");
            builder.Property(e => e.Timezone)
                .HasColumnName("Timezone")
                .HasDefaultValue("UTC");
            builder.Property(e => e.DateTimeFormat)
                .HasColumnName("DateTimeFormat")
                .HasDefaultValue("yyyy'-'MM'-'dd HH':'mm':'ss");
            builder.Property(e => e.DiscordUserId)
                .HasColumnName("DiscordUserId")
                .IsRequired(false);
            builder.Property(e => e.CheckAFK)
               .HasColumnName("CheckAFK");
            builder.Property(e => e.WindowsNotifications)
               .HasColumnName("WindowsNotifications");
        }
    }
}
