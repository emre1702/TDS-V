using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Shared.Data.Interfaces.Map.Creator;

namespace TDS.Shared.Data.Models.Map.Creator
{
    public class MapSharedLocation : IMapLocationData
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("ipls")]
        public List<string> Ipls { get; set; }

        [JsonProperty("iplsToUnload")]
        public List<string> IplsToUnload { get; set; }

        [JsonProperty("hash")]
        public ulong? Hash { get; set; }
    }
}