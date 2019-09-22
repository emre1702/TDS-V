using TDS_Client.Dto.Map;
using TDS_Common.Dto.Map.Creator;

namespace TDS_Server.Dto.Map
{
    public class Position4DDto
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float Rotation { get; set; }

        public Position4DDto() { }

        public Position4DDto(MapCreatorPosition pos) 
        {
            X = pos.PosX;
            Y = pos.PosY;
            Z = pos.PosZ;
            Rotation = pos.RotZ;
        }

        public Position3DDto To3D()
        {
            return new Position3DDto { X = X, Y = Y, Z = Z };
        }

        public MapCreatorPosition ToMapCreatorPosition(int id)
        {
            return new MapCreatorPosition
            {
                Id = id,
                PosX = X,
                PosY = Y,
                PosZ = Z,
                RotZ = Rotation
            };
        }
    }
}