using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Body
{
    public class PlayerBodyDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }

        [JsonProperty("0")]
        public virtual ICollection<PlayerBodyAppearanceDatas> AppearanceData { get; set; }

        [JsonProperty("1")]
        public virtual ICollection<PlayerBodyFeaturesDatas> FeaturesData { get; set; }

        [JsonProperty("2")]
        public virtual ICollection<PlayerBodyGeneralDatas> GeneralData { get; set; }

        [JsonProperty("3")]
        public virtual ICollection<PlayerBodyHairAndColorsDatas> HairAndColorsData { get; set; }

        [JsonProperty("4")]
        public virtual ICollection<PlayerBodyHeritageDatas> HeritageData { get; set; }
    }

    public class PlayerBodyDatasConfiguration : IEntityTypeConfiguration<PlayerBodyDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerBodyDatas> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.HasOne(e => e.Player)
                .WithOne(p => p.BodyDatas)
                .HasForeignKey<PlayerBodyDatas>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}