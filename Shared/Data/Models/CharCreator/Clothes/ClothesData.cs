using Newtonsoft.Json;
using TDS.Shared.Data.Default;

namespace TDS.Shared.Data.Models.CharCreator.Clothes
{
    public class ClothesData
    {
        [JsonProperty(ClothesDataKeyValue.Slot)]
        public byte Slot { get; set; }

        [JsonProperty(ClothesDataKeyValue.Hats)]
        public ClothesComponentOrPropData Hat { get; set; }

        [JsonProperty(ClothesDataKeyValue.Glasses)]
        public ClothesComponentOrPropData Glasses { get; set; }

        [JsonProperty(ClothesDataKeyValue.Masks)]
        public ClothesComponentOrPropData Mask { get; set; }

        [JsonProperty(ClothesDataKeyValue.Jackets)]
        public ClothesComponentOrPropData Jacket { get; set; }

        [JsonProperty(ClothesDataKeyValue.Shirts)]
        public ClothesComponentOrPropData Shirt { get; set; }

        [JsonProperty(ClothesDataKeyValue.Hands)]
        public ClothesComponentOrPropData Hands { get; set; }

        [JsonProperty(ClothesDataKeyValue.Accessories)]
        public ClothesComponentOrPropData Accessory { get; set; }

        [JsonProperty(ClothesDataKeyValue.Bags)]
        public ClothesComponentOrPropData Bag { get; set; }

        [JsonProperty(ClothesDataKeyValue.Legs)]
        public ClothesComponentOrPropData Legs { get; set; }

        [JsonProperty(ClothesDataKeyValue.Shoes)]
        public ClothesComponentOrPropData Shoes { get; set; }

        [JsonProperty(ClothesDataKeyValue.BodyArmors)]
        public ClothesComponentOrPropData BodyArmor { get; set; }

        [JsonProperty(ClothesDataKeyValue.Decals)]
        public ClothesComponentOrPropData Decal { get; set; }

        [JsonProperty(ClothesDataKeyValue.EarAccessories)]
        public ClothesComponentOrPropData EarAccessory { get; set; }

        [JsonProperty(ClothesDataKeyValue.Watches)]
        public ClothesComponentOrPropData Watch { get; set; }

        [JsonProperty(ClothesDataKeyValue.Bracelets)]
        public ClothesComponentOrPropData Bracelet { get; set; }
    }
}