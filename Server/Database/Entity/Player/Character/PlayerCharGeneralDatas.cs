using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character
{
    public class PlayerCharGeneralDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public bool IsMale { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }
    }

    public class PlayerCharGeneralDatasConfiguration : IEntityTypeConfiguration<PlayerCharGeneralDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerCharGeneralDatas> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.CharDatas)
                .WithMany(c => c.GeneralData)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
