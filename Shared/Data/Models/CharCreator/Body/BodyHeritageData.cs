using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.CharCreator.Body
{
    public class BodyHeritageData
    {
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
    }
}