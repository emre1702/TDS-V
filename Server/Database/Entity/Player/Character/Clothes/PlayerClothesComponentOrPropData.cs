using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Clothes
{
    public class PlayerClothesComponentOrPropData : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonIgnore]
        public byte Slot { get; set; }

        [JsonIgnore]
        public ClothesDataKey Key { get; set; }

        [JsonProperty("0")]
        public int DrawableId { get; set; }

        [JsonProperty("1")]
        public int TextureId { get; set; }
    }

    public class PlayerClothesComponentOrPropDataConfiguration : IEntityTypeConfiguration<PlayerClothesComponentOrPropData>
    {
        public void Configure(EntityTypeBuilder<PlayerClothesComponentOrPropData> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot, e.Key });
        }
    }
}