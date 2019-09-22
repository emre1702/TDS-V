using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Common.Dto.Map.Creator
{
    public class MapCreateDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EMapType Type { get; set; }
        public uint MinPlayers { get; set; }
        public uint MaxPlayers { get; set; }
        public Dictionary<int, string> Description { get; set; }
        public MapCreatorPosition[] Objects { get; set; }
        public MapCreatorPosition[][] TeamSpawns { get; set; }
        public MapCreatorPosition[] MapEdges { get; set; }
        public MapCreatorPosition[] BombPlaces { get; set; }
        public MapCreatorPosition MapCenter { get; set; }

    }
}
