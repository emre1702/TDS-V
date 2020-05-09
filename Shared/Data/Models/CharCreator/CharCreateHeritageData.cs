using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.CharCreator
{
    public class CharCreateHeritageData
    {
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
