using NeoSmart.Hashing.XXHash;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using TDS_Shared.Data.Enums;

namespace TDS_Shared.Data.Utility
{
    public static class SharedUtils
    {
        #region Public Fields

        public static readonly Random Rnd = new Random();

        #endregion Public Fields

        #region Public Properties

        public static bool IsServersided { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Color? GetColorFromHtmlRgba(string rgba)
        {
            if (string.IsNullOrEmpty(rgba))
                return null;

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

        public static T GetRandom<T>(params T[] elements)
        {
            var rndIndex = Rnd.Next(0, elements.Length);
            return elements[rndIndex];
        }

        public static T GetRandom<T>(List<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection[rndIndex];
        }

        public static T GetRandom<T>(HashSet<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection.ElementAt(rndIndex);
        }

        public static T GetRandom<T>(ICollection<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection.ElementAt(rndIndex);
        }

        public static string HashPWClient(string pw)
        {
            return XXHash64.Hash(Encoding.Default.GetBytes(pw)).ToString();
        }

        #endregion Public Methods
    }
}
