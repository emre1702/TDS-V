using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS_Server.Data.Extensions;

namespace TDS_Server.Data.Models.GTA
{
    public class Position3D
    {
        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float X { get; set; }

        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float Y { get; set; }

        [XmlAttribute("x")]
        [JsonProperty("0")]
        public float Z { get; set; }

        private static readonly Random _random = new Random();

        public Position3D(float x, float y, float z) 
            => (X, Y, Z) = (x, y, z);

        public Position3D(double x, double y, double z)
           => (X, Y, Z) = ((float)x, (float)y, (float)z);

        public Position3D(int x, int y, int z)
           => (X, Y, Z) = (x, y, z);

        public void Around(float around, bool considerZ = false)
        {
            float addToX = _random.NextFloat(-around, around);
            X += addToX;
            around -= Math.Abs(addToX);

            if (around == 0)
                return;

            if (!considerZ)
            { 
                Y += _random.NextDouble() >= 0.5 ? around : -around;
                return;
            }

            float addToY = _random.NextFloat(-around, around);
            Y += addToY;
            around -= addToY;

            if (around == 0)
                return;

            Z += _random.NextDouble() >= 0.5 ? around : -around;
        }
    }
}
