using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Dto.Map;

namespace TDS_Server.Dto.Map.Creator
{
    public class MapCreatorPosition
    {
        public int Id { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }

        public MapCreatorPosition() { }

        public MapCreatorPosition(int id, Position3DDto pos)
        {
            Id = id;
            PosX = pos.X;
            PosY = pos.Y;
            PosZ = pos.Z;
        }

        public MapCreatorPosition(int id, Position4DDto pos)
        {
            Id = id;
            PosX = pos.X;
            PosY = pos.Y;
            PosZ = pos.Z;
            RotZ = pos.Rotation;
        }
    }
}
