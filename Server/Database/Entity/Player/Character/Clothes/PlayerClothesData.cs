using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Server.Database.Entity.Player.Character.Clothes
{
    public class PlayerClothesData : IPlayerDataTable
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty(ClothesDataKeyValue.Slot)]
        public byte Slot { get; set; }

        [JsonIgnore]
        public PlayerClothesDatas ClothesDatas { get; set; }

        [JsonProperty(ClothesDataKeyValue.Hats)]
        public PlayerClothesComponentOrPropData Hat { get; set; }

        [JsonProperty(ClothesDataKeyValue.Glasses)]
        public PlayerClothesComponentOrPropData Glasses { get; set; }

        [JsonProperty(ClothesDataKeyValue.Masks)]
        public PlayerClothesComponentOrPropData Mask { get; set; }

        [JsonProperty(ClothesDataKeyValue.Jackets)]
        public PlayerClothesComponentOrPropData Jacket { get; set; }

        [JsonProperty(ClothesDataKeyValue.Shirts)]
        public PlayerClothesComponentOrPropData Shirt { get; set; }

        [JsonProperty(ClothesDataKeyValue.Hands)]
        public PlayerClothesComponentOrPropData Hands { get; set; }

        [JsonProperty(ClothesDataKeyValue.Accessories)]
        public PlayerClothesComponentOrPropData Accessory { get; set; }

        [JsonProperty(ClothesDataKeyValue.Bags)]
        public PlayerClothesComponentOrPropData Bag { get; set; }

        [JsonProperty(ClothesDataKeyValue.Legs)]
        public PlayerClothesComponentOrPropData Legs { get; set; }

        [JsonProperty(ClothesDataKeyValue.Shoes)]
        public PlayerClothesComponentOrPropData Shoes { get; set; }

        [JsonProperty(ClothesDataKeyValue.BodyArmors)]
        public PlayerClothesComponentOrPropData BodyArmor { get; set; }

        [JsonProperty(ClothesDataKeyValue.Decals)]
        public PlayerClothesComponentOrPropData Decal { get; set; }

        [JsonProperty(ClothesDataKeyValue.EarAccessories)]
        public PlayerClothesComponentOrPropData EarAccessory { get; set; }

        [JsonProperty(ClothesDataKeyValue.Watches)]
        public PlayerClothesComponentOrPropData Watch { get; set; }

        [JsonProperty(ClothesDataKeyValue.Bracelets)]
        public PlayerClothesComponentOrPropData Bracelet { get; set; }
    }

    public class PlayerClothesDataConfiguration : IEntityTypeConfiguration<PlayerClothesData>
    {
        public void Configure(EntityTypeBuilder<PlayerClothesData> builder)
        {
            builder.HasKey(e => new { e.PlayerId, e.Slot });

            builder.HasOne(e => e.ClothesDatas)
                .WithMany(c => c.DatasPerSlot)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Accessory)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Accessories })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Bag)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Bags })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.BodyArmor)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.BodyArmors })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Bracelet)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Bracelets })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Decal)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Decals })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EarAccessory)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.EarAccessories })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Glasses)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Glasses })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Hands)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Hands })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Hat)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Hats })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Jacket)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Jackets })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Legs)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Legs })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Mask)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Masks })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Shirt)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Shirts })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Shoes)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Shoes })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Watch)
                .WithOne()
                .HasForeignKey<PlayerClothesData>(e => new { e.PlayerId, e.Slot, ClothesDataKey.Watches })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}