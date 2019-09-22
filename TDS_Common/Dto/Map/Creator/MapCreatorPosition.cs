using TDS_Common.Enum;

namespace TDS_Common.Dto.Map.Creator
{
    public class MapCreatorPosition
    {
        public int Id { get; set; }
        public EMapCreatorPositionType Type { get; set; }
        public object Info { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }

        public MapCreatorPosition() { }
    }
}
