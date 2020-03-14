using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Shared.Data.Enums;

namespace TDS_Shared.Data.Models.Map
{
    public class BrowserSyncedMapDataDto
    {
        [JsonProperty("0")]
        public int Id = 0;
        [JsonProperty("1")]
        public string Name = "unknown";
        [JsonProperty("2")]
        public MapType Type = MapType.Normal;
        [JsonProperty("3")]
        public Dictionary<int, string> Description = new Dictionary<int, string>
        {
            [(int)Language.English] = "No info available.",
            [(int)Language.German] = "Keine Info verfügbar."
        };
        [JsonProperty("4")]
        public string CreatorName;
        [JsonProperty("5")]
        public uint Rating = 5;
    }
}
