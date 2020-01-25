using Newtonsoft.Json;

namespace TDS_Common.Dto
{
    public class ColorDto
    {
        [JsonProperty("0")]
        public short R { get; set; }
        [JsonProperty("1")]
        public short G { get; set; }
        [JsonProperty("2")]
        public short B { get; set; }
        [JsonProperty("3")]
        public short A { get; set; } = 255;

        public ColorDto(short r, short g, short b, short a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
