using Newtonsoft.Json;

namespace TDS_Server.Data.Models.GangWindow
{
    public class GangCreateData
    {
        [JsonProperty("0")]
        public string Name { get; set; }

        [JsonProperty("1")]
        public string Short { get; set; }

        [JsonProperty("2")]
        public string Color { get; set; }

        [JsonProperty("3")]
        public byte BlipColor { get; set; }
    }
}
