using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player.Character.Clothes
{
    public class PlayerClothesDatas : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("1")]
        public byte SelectedSlot { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }

        [JsonProperty("0")]
        public virtual ICollection<PlayerClothesData> DatasPerSlot { get; set; }
    }

    public class PlayerClothesDatasConfiguration : IEntityTypeConfiguration<PlayerClothesDatas>
    {
        public void Configure(EntityTypeBuilder<PlayerClothesDatas> builder)
        {
            builder.HasKey(e => e.PlayerId);

            builder.HasOne(e => e.Player)
                .WithOne(c => c.ClothesDatas)
                .HasForeignKey<PlayerClothesDatas>(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}