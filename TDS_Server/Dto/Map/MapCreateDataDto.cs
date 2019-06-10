using System.Collections.Generic;
using TDS_Common.Enum;
using EMapType = TDS_Server.Enum.EMapType;

namespace TDS_Common.Dto.Map
{
    public class MapCreateDataDto
    {
        #nullable disable

        public string Name { get; set; }
        public EMapType Type { get; set; }
        public uint MinPlayers { get; set; }
        public uint MaxPlayers { get; set; }
        public Dictionary<ELanguage, string> Description { get; set; }
        public Position4DDto[][] TeamSpawns { get; set; }
        public Position3DDto[] MapEdges { get; set; }
        public Position3DDto[] BombPlaces { get; set; }
        public Position3DDto MapCenter { get; set; }

        #nullable restore
    }
}
