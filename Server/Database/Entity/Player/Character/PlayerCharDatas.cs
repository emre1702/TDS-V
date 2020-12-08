using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character
{
    public class PlayerCharDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }
        [JsonProperty("0")]
        public virtual ICollection<PlayerCharAppearanceDatas> AppearanceData { get; set; }
        [JsonProperty("1")]
        public virtual ICollection<PlayerCharFeaturesDatas> FeaturesData { get; set; }
        [JsonProperty("2")]
        public virtual ICollection<PlayerCharGeneralDatas> GeneralData { get; set; }
        [JsonProperty("3")]
        public virtual ICollection<PlayerCharHairAndColorsDatas> HairAndColorsData { get; set; }
        [JsonProperty("4")]
        public virtual ICollection<PlayerCharHeritageDatas> HeritageData { get; set; }
    }

    public class PlayerCharDatasConfiguration : IEntityTypeConfiguration<PlayerCharDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerCharDatas> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.HasOne(e => e.Player)
                .WithOne(p => p.CharDatas)
                .HasForeignKey<PlayerCharDatas>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
