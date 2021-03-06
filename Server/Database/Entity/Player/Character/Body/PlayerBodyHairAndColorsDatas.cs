﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Body
{
    public class PlayerBodyHairAndColorsDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public int Hair { get; set; }

        [JsonProperty("1")]
        public int HairColor { get; set; }

        [JsonProperty("2")]
        public int HairHighlightColor { get; set; }

        [JsonProperty("3")]
        public int EyebrowColor { get; set; }

        [JsonProperty("4")]
        public int FacialHairColor { get; set; }

        [JsonProperty("5")]
        public int EyeColor { get; set; }

        [JsonProperty("6")]
        public int BlushColor { get; set; }

        [JsonProperty("7")]
        public int LipstickColor { get; set; }

        [JsonProperty("8")]
        public int ChestHairColor { get; set; }

        [JsonIgnore]
        public virtual PlayerBodyDatas BodyDatas { get; set; }
    }

    public class PlayerBodyHairAndColorsDatasConfiguration : IEntityTypeConfiguration<PlayerBodyHairAndColorsDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerBodyHairAndColorsDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.BodyDatas)
                .WithMany(c => c.HairAndColorsData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}