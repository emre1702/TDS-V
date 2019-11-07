using NeoSmart.Hashing.XXHash;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace TDS_Common.Manager.Utility
{
    public static class CommonUtils
    {
        public static readonly Random Rnd = new Random();
        public static bool IsServersided { get; set; }

        public static string HashPWClient(string pw)
        {
            return XXHash64.Hash(Encoding.Default.GetBytes(pw)).ToString();
        }

        public static T GetRandom<T>(params T[] elements)
        {
            var rndIndex = Rnd.Next(0, elements.Length);
            return elements[rndIndex];
        }

        public static Color GetColorFromHtmlRgba(string rgba)
        {
            int left = rgba.IndexOf('(');
            int right = rgba.IndexOf(')');

            if (left < 0 || right < 0)
                return Color.White;
            string noBrackets = rgba.Substring(left + 1, right - left - 1);

            string[] parts = noBrackets.Split(',');

            int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
            int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
            int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

            if (parts.Length == 3)
            {
                return Color.FromArgb(r, g, b);
            }
            else if (parts.Length == 4)
            {
                float a = float.Parse(parts[3], CultureInfo.InvariantCulture);
                return Color.FromArgb((int)(a * 255), r, g, b);
            }

            return Color.White;
        }
    }
}