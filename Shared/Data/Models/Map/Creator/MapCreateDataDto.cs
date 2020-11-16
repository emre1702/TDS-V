using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS.Shared.Data.Enums;

namespace TDS.Shared.Data.Models.Map.Creator
{
    public class MapCreateDataDto
    {
        [JsonProperty("0")]
        public int Id { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; }
        [JsonProperty("2")]
        public MapType Type { get; set; }
        [JsonProperty("3")]
        public MapCreateSettings Settings { get; set; }
        [JsonProperty("4")]
        public Dictionary<int, string> Description { get; set; }
        [JsonProperty("5")]
        public List<MapCreatorPosition> Objects { get; set; }
        [JsonProperty("6")]
        public List<List<MapCreatorPosition>> TeamSpawns { get; set; }
        [JsonProperty("7")]
        public List<MapCreatorPosition> MapEdges { get; set; }
        [JsonProperty("8")]
        public List<MapCreatorPosition> BombPlaces { get; set; }
        [JsonProperty("9")]
        public MapCreatorPosition MapCenter { get; set; }
        [JsonProperty("10")]
        public MapCreatorPosition Target { get; set; }
        [JsonProperty("11")]
        public List<MapCreatorPosition> Vehicles { get; set; }

        [JsonIgnore]
        public List<MapCreatorPosition> AllPositions 
        {    
            get 
            {
                var list = new List<MapCreatorPosition>();
                if (!(Objects is null))
                    list.AddRange(Objects);
                if (!(TeamSpawns is null))
                    list.AddRange(TeamSpawns.SelectMany(s => s));
                if (!(MapEdges is null))
                    list.AddRange(MapEdges);
                if (!(BombPlaces is null))
                    list.AddRange(BombPlaces);
                if (!(Vehicles is null))
                    list.AddRange(Vehicles);
                if (!(MapCenter is null))
                    list.Add(MapCenter);
                if (!(Target is null))
                    list.Add(Target);

                return list;
            }
        }

    }
}
