using MessagePack;
using TDS_Common.Enum;

namespace TDS_Common.Dto.Map.Creator
{
    [MessagePackObject]
    public class MapCreatorPosition
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public EMapCreatorPositionType Type { get; set; }
        [Key(2)]
        public object Info { get; set; }
        [Key(3)]
        public float PosX { get; set; }
        [Key(4)]
        public float PosY { get; set; }
        [Key(5)]
        public float PosZ { get; set; }
        [Key(6)]
        public float RotX { get; set; }
        [Key(7)]
        public float RotY { get; set; }
        [Key(8)]
        public float RotZ { get; set; }

        public MapCreatorPosition() { }
    }
}
