namespace TDS_Common.Dto
{
    public class ColorDto
    {
        public short R { get; set; }
        public short G { get; set; }
        public short B { get; set; }
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
