using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Shared.Data.Interfaces.Map.Creator;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Shared.Data.Models.Map.Creator
{
    public class LocationData : IMapLocationData
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("ipls")]
        public List<string> Ipls { get; set; }

        [JsonProperty("iplsToUnload")]
        public List<string> IplsToUnload { get; set; }

        [JsonProperty("position")]
        public Position3D Position { get; set; }
    }
}