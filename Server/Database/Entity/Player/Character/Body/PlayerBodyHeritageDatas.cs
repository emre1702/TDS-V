using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Body
{
    public class PlayerBodyHeritageDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public int FatherIndex { get; set; }

        [JsonProperty("1")]
        public int MotherIndex { get; set; }

        [JsonProperty("2")]
        public float ResemblancePercentage { get; set; }

        [JsonProperty("3")]
        public float SkinTonePercentage { get; set; }

        [JsonIgnore]
        public virtual PlayerBodyDatas BodyDatas { get; set; }
    }

    public class PlayerBodyHeritageDatasConfiguration : IEntityTypeConfiguration<PlayerBodyHeritageDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerBodyHeritageDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.BodyDatas)
                .WithMany(c => c.HeritageData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}