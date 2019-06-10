using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Common.Dto.Map
{
    public class MapCreateDataDto
    {
        public string Name { get; set; }
        public EMapType Type { get; set; }
        public Dictionary<ELanguage, string> Description { get; set; }
        public Position4DDto[] TeamSpawns { get; set; }
        public Position2DDto[] MapEdges { get; set; }
        public Position4DDto[] BombPlaces { get; set; }
        public Position2DDto MapCenter { get; set; }
    }
}
