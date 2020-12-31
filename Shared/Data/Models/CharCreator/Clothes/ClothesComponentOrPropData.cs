using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.CharCreator.Clothes
{
    public class ClothesComponentOrPropData
    {
        [JsonProperty("0")]
        public int DrawableId { get; set; }

        [JsonProperty("1")]
        public int TextureId { get; set; }
    }
}