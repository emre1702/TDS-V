using MessagePack;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;

namespace TDS_Common.Dto.Map.Creator
{
    [MessagePackObject]
    public class MapCreateDataDto
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public EMapType Type { get; set; }
        [Key(3)]
        public MapCreateSettings Settings { get; set; }
        [Key(4)]
        public Dictionary<int, string> Description { get; set; }
        [Key(5)]
        public List<MapCreatorPosition> Objects { get; set; }
        [Key(6)]
        public List<List<MapCreatorPosition>> TeamSpawns { get; set; }
        [Key(7)]
        public List<MapCreatorPosition> MapEdges { get; set; }
        [Key(8)]
        public List<MapCreatorPosition> BombPlaces { get; set; }
        [Key(9)]
        public MapCreatorPosition MapCenter { get; set; }
        [Key(10)]
        public MapCreatorPosition Target { get; set; }

        [IgnoreMember]
        public List<MapCreatorPosition> GetAllPositions 
        {    
            get 
            {
                var list = Objects
                    .Concat(TeamSpawns.SelectMany(s => s).ToList())
                    .Concat(MapEdges)
                    .Concat(BombPlaces)
                    .ToList();
                list.Add(MapCenter);
                list.Add(Target);

                return list;
            }
        }

    }
}
