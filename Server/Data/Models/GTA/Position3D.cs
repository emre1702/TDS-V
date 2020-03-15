using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TDS_Shared.Data.Extensions;
using TDS_Shared.Data.Utility;

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
    }
}
