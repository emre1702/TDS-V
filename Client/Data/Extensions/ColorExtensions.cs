using System;
using System.Drawing;

namespace TDS_Client.Data.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetContrast(this Color original)
        {
            var l = 0.2126 * (original.R / 255d) + 0.7152 * (original.G / 255d) + 0.0722 * (original.B / 255d);

            return l < 0.4 ? Color.White : Color.Black;
        }

        public static Color GetBetween(this Color first, Color second, float percentage = 0.5f)
        {
            return Color.FromArgb(
                    (int)Math.Abs(second.A + (first.A - second.A) * percentage),
                    (int)Math.Abs(second.R + (first.R - second.R) * percentage),
                    (int)Math.Abs(second.G + (first.G - second.G) * percentage),
                    (int)Math.Abs(second.B + (first.B - second.B) * percentage)
                );
        }

    }
}
