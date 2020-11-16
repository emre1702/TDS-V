using Newtonsoft.Json;

namespace TDS.Server.Data.Models.GangWindow
{
    public class GangCreateData
    {
        [JsonProperty("0")]
        public string Name { get; set; }

        [JsonProperty("1")]
#pragma warning disable CA1720 // Identifier contains type name
        public string Short { get; set; }
#pragma warning restore CA1720 // Identifier contains type name

        [JsonProperty("2")]
        public string Color { get; set; }

        [JsonProperty("3")]
        public byte BlipColor { get; set; }
    }
}
