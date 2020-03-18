using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS_Shared.Data.Extensions;
using TDS_Shared.Data.Utility;

namespace TDS_Shared.Data.Models.GTA
{
    public class Position3D
    {
        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        [JsonProperty("2")]
        public float Z { get; set; }

        public Position3D() { }

        public Position3D(float x, float y, float z)
            => (X, Y, Z) = (x, y, z);

        public Position3D(double x, double y, double z)
           => (X, Y, Z) = ((float)x, (float)y, (float)z);

        public Position3D(int x, int y, int z)
           => (X, Y, Z) = (x, y, z);

        public Position3D Around(float around, bool considerZ = false)
        {
            float addToX = SharedUtils.Rnd.NextFloat(-around, around);
            X += addToX;
            around -= Math.Abs(addToX);

            if (around == 0)
                return this;

            if (!considerZ)
            {
                Y += SharedUtils.GetRandom(true, false) ? around : -around;
                return this;
            }

            float addToY = SharedUtils.Rnd.NextFloat(-around, around);
            Y += addToY;
            around -= addToY;

            if (around == 0)
                return this;

            Z += SharedUtils.GetRandom(true, false) ? around : -around;

            return this;
        }

        public float DistanceTo(Position3D otherPos)
        {
            var dX = X - otherPos.X;
            var dY = Y - otherPos.Y;
            var dZ = Z - otherPos.Z;
            return (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public float DistanceTo2D(Position3D otherPos)
        {
            var dX = X - otherPos.X;
            var dY = Y - otherPos.Y;
            return (float)Math.Sqrt(dX * dX + dY * dY);
        }
    }
}
