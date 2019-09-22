using TDS_Common.Dto.Map.Creator;

namespace TDS_Client.Dto.Map
{
    public class Position3DDto
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Position3DDto() { }

        public Position3DDto(MapCreatorPosition pos)
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
        }

        public MapCreatorPosition ToMapCreatorPosition(int id)
        {
            return new MapCreatorPosition
            {
                Id = id,
                PosX = X,
                PosY = Y,
                PosZ = Z
            };
        }
    }
}