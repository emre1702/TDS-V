using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Interfaces;

namespace TDS_Server.Database.Entity.Player.Settings
{
    public class PlayerKillInfoSettings : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("0")]
        public bool ShowIcon { get; set; }

        [JsonProperty("1")]
        public float FontSize { get; set; }

        [JsonProperty("2")]
        public int IconWidth { get; set; }

        [JsonProperty("3")]
        public int Spacing { get; set; }

        [JsonProperty("4")]
        public float Duration { get; set; }

        [JsonProperty("5")]
        public int IconHeight { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }
    }

    internal class PlayerKillInfoSettingsConfiguration : IEntityTypeConfiguration<PlayerKillInfoSettings>
    {
        public void Configure(EntityTypeBuilder<PlayerKillInfoSettings> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.Property(e => e.PlayerId)
                .ValueGeneratedNever();

            builder.Property(e => e.FontSize)
                .IsRequired()
                .HasDefaultValue(1.4f);

            builder.Property(e => e.IconWidth)
                .IsRequired()
                .HasDefaultValue(60);

            builder.Property(e => e.IconHeight)
                .IsRequired()
                .HasDefaultValue(30);

            builder.Property(e => e.Spacing)
                .IsRequired()
                .HasDefaultValue(15);

            builder.Property(e => e.Duration)
                .IsRequired()
                .HasDefaultValue(10);

            builder.HasOne(e => e.Player)
                .WithOne(p => p.KillInfoSettings)
                .HasForeignKey<PlayerKillInfoSettings>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
