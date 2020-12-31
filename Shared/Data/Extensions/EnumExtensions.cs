using System;

namespace TDS.Shared.Data.Extensions
{
    public static class EnumExtensions
    {
        public static string ToNumberString(this Enum val)
            => val.ToString("D");
    }
}