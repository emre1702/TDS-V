using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class ColorDto
    {
        [Key(0)]
        public short R { get; set; }
        [Key(1)]
        public short G { get; set; }
        [Key(2)]
        public short B { get; set; }
        [Key(3)]
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
