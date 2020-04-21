using AutoMapper;
using System;
using System.Globalization;

namespace TDS_Server.Handler.Converter.Mapping
{
    class StringToTimeSpanConverter : ITypeConverter<string, TimeSpan?>
    {
        public TimeSpan? Convert(string time, TimeSpan? destination, ResolutionContext context)
        {
            return GetTime(time);
        }

        private TimeSpan? GetTime(string time)
        {
            switch (time)
            {
                #region Seconds

                case string _ when time.EndsWith("s", true, CultureInfo.CurrentCulture):    // seconds
                    if (!double.TryParse(time[0..^1], out double seconds))
                        return null;
                    return TimeSpan.FromSeconds(seconds); ;

                case string _ when time.EndsWith("sec", true, CultureInfo.CurrentCulture):    // seconds
                    if (!double.TryParse(time[0..^3], out double secs))
                        return null;
                    return TimeSpan.FromSeconds(secs);

                #endregion Seconds

                #region Minutes

                case string _ when time.EndsWith("m", true, CultureInfo.CurrentCulture):    // minutes
                    if (!double.TryParse(time[0..^1], out double minutes))
                        return null;
                    return TimeSpan.FromMinutes(minutes);

                case string _ when time.EndsWith("min", true, CultureInfo.CurrentCulture):    // minutes
                    if (!double.TryParse(time[0..^3], out double mins))
                        return null;
                    return TimeSpan.FromMinutes(mins);

                #endregion Minutes

                #region Hours

                case string _ when time.EndsWith("h", true, CultureInfo.CurrentCulture):    // hours
                    if (!double.TryParse(time[0..^1], out double hours))
                        return null;
                    return TimeSpan.FromHours(hours);

                case string _ when time.EndsWith("st", true, CultureInfo.CurrentCulture):    // hours
                    if (!double.TryParse(time[0..^2], out double hours2))
                        return null;
                    return TimeSpan.FromHours(hours2);

                #endregion Hours

                #region Days

                case string _ when time.EndsWith("d", true, CultureInfo.CurrentCulture):    // days
                case string _ when time.EndsWith("t", true, CultureInfo.CurrentCulture):    // days
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

        private bool IsPerma(string time)
        {
            return time == "-1"
                || time == "-"
                || time.Equals("perma", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("permamute", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("permaban", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("never", StringComparison.CurrentCultureIgnoreCase);
        }

        private bool IsUnmute(string time)
        {
            return time == "0"
                || time.Equals("unmute", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("unban", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("stop", StringComparison.CurrentCultureIgnoreCase)
                || time.Equals("no", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
