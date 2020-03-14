using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Shared.Data.Models.Map
{
    public class ClientSyncedDataDto
    {
        [JsonProperty("0")]
        public string Name { get; set; }

        [JsonProperty("1")]
        public List<Position3DDto> BombPlaces { get; set; }

        [JsonProperty("2")]
        public Position3DDto Center { get; set; }

        [JsonProperty("3")]
        public List<MapCreatorPosition> Objects { get; set; }

        [JsonProperty("4")]
        public List<MapCreatorPosition> Vehicles { get; set; }

        [JsonProperty("5")]
        public Position3DDto Target { get; set; }

        [JsonProperty("6")]
        public List<Position3DDto> MapEdges { get; set; }
    }
}
