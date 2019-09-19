using System.Collections.Generic;
using TDS_Common.Enum;
using EMapType = TDS_Server.Enum.EMapType;

namespace TDS_Server.Dto.Map.Creator
{
    public class MapCreateDataDto
    {
        #nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public EMapType Type { get; set; }
        public uint MinPlayers { get; set; }
        public uint MaxPlayers { get; set; }
        public Dictionary<ELanguage, string> Description { get; set; }
        public MapCreatorObject[] Objects { get; set; }
        public MapCreatorPosition[][] TeamSpawns { get; set; }
        public MapCreatorPosition[] MapEdges { get; set; }
        public MapCreatorPosition[] BombPlaces { get; set; }
        public MapCreatorPosition MapCenter { get; set; }

        #nullable restore
    }
}
