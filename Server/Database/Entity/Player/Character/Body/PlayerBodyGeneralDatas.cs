using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Body
{
    public class PlayerBodyGeneralDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public bool IsMale { get; set; }

        [JsonIgnore]
        public virtual PlayerBodyDatas BodyDatas { get; set; }
    }

    public class PlayerBodyGeneralDatasConfiguration : IEntityTypeConfiguration<PlayerBodyGeneralDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerBodyGeneralDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.BodyDatas)
                .WithMany(c => c.GeneralData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}