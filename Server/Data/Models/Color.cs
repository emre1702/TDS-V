using AltV.Net;
using Newtonsoft.Json;

namespace TDS_Server.Data.Models
{
    public class Color : IWritable
    {
        [JsonProperty("0")]
        public byte R { get; set; }
        [JsonProperty("1")]
        public byte G { get; set; }
        [JsonProperty("2")]
        public byte B { get; set; }
        [JsonProperty("3")]
        public byte A { get; set; } = 255;

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString()
        {
            return $"({R}, {G}, {B}, {A})";
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            writer.Name("R");
            writer.Value(R);

            writer.Name("G");
            writer.Value(G);

            writer.Name("B");
            writer.Value(B);

            writer.Name("A");
            writer.Value(A);

            writer.EndObject();
        }
    }
}
