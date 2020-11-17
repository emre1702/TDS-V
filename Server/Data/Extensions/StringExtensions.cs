using System;
using System.Globalization;

namespace TDS.Server.Data.Extensions
{
    public static class StringExtensions
    {
        public static DateTime? ParseDateTime(this string time)
        {
            time = time.ToLower();

            switch (time)
            {
                #region Seconds

                case string _ when time.EndsWith("s"):    // seconds
                    if (!double.TryParse(time[0..^1], out double seconds))
                        return null;
                    return DateTime.UtcNow.AddSeconds(seconds);

                case string _ when time.EndsWith("sec"):    // seconds
                    if (!double.TryParse(time[0..^3], out double secs))
                        return null;
                    return DateTime.UtcNow.AddSeconds(secs);

                #endregion Seconds

                #region Minutes

                case string _ when time.EndsWith("m"):    // minutes
                    if (!double.TryParse(time[0..^1], out double minutes))
                        return null;
                    return DateTime.UtcNow.AddMinutes(minutes);

                case string _ when time.EndsWith("min"):    // minutes
                    if (!double.TryParse(time[0..^3], out double mins))
                        return null;
                    return DateTime.UtcNow.AddMinutes(mins);

                #endregion Minutes

                #region Hours

                case string _ when time.EndsWith("h"):    // hours
                    if (!double.TryParse(time[0..^1], out double hours))
                        return null;
                    return DateTime.UtcNow.AddHours(hours);

                case string _ when time.EndsWith("st"):    // hours
                    if (!double.TryParse(time[0..^2], out double hours2))
                        return null;
                    return DateTime.UtcNow.AddHours(hours2);

                #endregion Hours

                #region Days

                case string _ when time.EndsWith("d"):    // days
                case string _ when time.EndsWith("t"):    // days
                    if (!double.TryParse(time[0..^1], out double days))
                        return null;
                    return DateTime.UtcNow.AddDays(days);

                #endregion Days

                #region Perma

                case string _ when IsPerma(time):       // perma
                    return DateTime.MaxValue;

                #endregion Perma

                #region Unmute

                case string _ when IsUnmute(time):       // unmute
                    return DateTime.MinValue;

                #endregion Unmute

                default:
                    return null;
            };
        }

        public static TimeSpan? ParseTimeSpan(this string time)
        {
            time = time.ToLower();

            switch (time)
            {
                #region Seconds

                case string _ when time.EndsWith("s"):    // seconds
                    if (!double.TryParse(time[0..^1], out double seconds))
                        return null;
                    return TimeSpan.FromSeconds(seconds);

                case string _ when time.EndsWith("sec"):    // seconds
                    if (!double.TryParse(time[0..^3], out double secs))
                        return null;
                    return TimeSpan.FromSeconds(secs);

                #endregion Seconds

                #region Minutes

                case string _ when time.EndsWith("m"):    // minutes
                    if (!double.TryParse(time[0..^1], out double minutes))
                        return null;
                    return TimeSpan.FromMinutes(minutes);

                case string _ when time.EndsWith("min"):    // minutes
                    if (!double.TryParse(time[0..^3], out double mins))
                        return null;
                    return TimeSpan.FromMinutes(mins);

                #endregion Minutes

                #region Hours

                case string _ when time.EndsWith("h"):    // hours
                    if (!double.TryParse(time[0..^1], out double hours))
                        return null;
                    return TimeSpan.FromHours(hours);

                case string _ when time.EndsWith("st"):    // hours
                    if (!double.TryParse(time[0..^2], out double hours2))
                        return null;
                    return TimeSpan.FromHours(hours2);

                #endregion Hours

                #region Days

                case string _ when time.EndsWith("d"):    // days
                case string _ when time.EndsWith("t"):    // days
                    if (!double.TryParse(time[0..^1], out double days))
                        return null;
                    return TimeSpan.FromDays(days);

                #endregion Days

                #region Perma

                case string _ when IsPerma(time):       // perma
                    return TimeSpan.MaxValue;

                #endregion Perma

                #region Unmute

                case string _ when IsUnmute(time):       // unmute
                    return TimeSpan.MinValue;

                #endregion Unmute

                default:
                    return null;
            };
        }

        private static bool IsPerma(string time)
        {
            return time == "-1"
                || time == "-"
                || time == "perma"
                || time == "permamute"
                || time == "permaban"
                || time == "never";
        }

        private static bool IsUnmute(string time)
        {
            return time == "0"
                || time == "unmute"
                || time == "unban"
                || time == "stop"
                || time == "no";
        }

        public static string TrimAndRemoveDuplicateSpaces(this string str)
        {
            while (str.Contains("  "))
                str = str.Replace("  ", " ");
            return str.Trim();
        }
    }
}
