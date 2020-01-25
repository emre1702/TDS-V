using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    public class SyncedMapDataDto
    {
        [JsonProperty("0")]
        public int Id = 0;
        [JsonProperty("1")]
        public string Name = "unknown";
        [JsonProperty("2")]
        public EMapType Type = EMapType.Normal;
        [JsonProperty("3")]
        public Dictionary<int, string> Description = new Dictionary<int, string>
        {
            [(int)ELanguage.English] = "No info available.",
            [(int)ELanguage.German] = "Keine Info verfügbar."
        };
        [JsonProperty("4")]
        public string CreatorName;
        [JsonProperty("5")]
        public uint Rating = 5;
    }
}
