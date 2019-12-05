using MessagePack;

namespace TDS_Common.Dto.Map.Creator
{
    [MessagePackObject]
    public class MapCreatorPosData
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public float PosX { get; set; }
        [Key(2)]
        public float PosY { get; set; }
        [Key(3)]
        public float PosZ { get; set; }
        [Key(4)]
        public float RotX { get; set; }
        [Key(5)]
        public float RotY { get; set; }
        [Key(6)]
        public float RotZ { get; set; }
    }
}
