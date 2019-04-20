using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto
{
    public class PositionRotationDto
    {
        public Vector3 Position;
        public float Rotation;

        public Vector3 RotationVector { get => new Vector3(0, 0, Rotation); }

        public PositionRotationDto(Vector3 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
