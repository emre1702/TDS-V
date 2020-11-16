using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.GTA
{
    public class Position2D
    {
        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        public Position2D() { }

        public Position2D(float x, float y)
            => (X, Y) = (x, y);

        public Position2D(double x, double y)
           => (X, Y) = ((float)x, (float)y);

        public Position2D(int x, int y)
           => (X, Y) = (x, y);

    }
}
