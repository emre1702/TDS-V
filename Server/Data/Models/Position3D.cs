using System;

namespace TDS_Server.Data.Models
{
    public class Position3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        private static Random _random;

        public void Around(float around, bool considerZ = false)
        {

        }
    }
}
