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
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Bag)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.BodyArmor)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Bracelet)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Decal)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EarAccessory)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Glasses)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Hands)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Hat)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Jacket)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Legs)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Mask)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Shirt)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Shoes)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Watch)
                .WithOne()
                .HasForeignKey<PlayerClothesComponentOrPropData>(e => new { e.PlayerId, e.Slot })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}