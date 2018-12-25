using System;

namespace TDS_Common.Manager.Utility
{
    public class CommonUtils
    {
        public static string MSToMinutesSeconds(int ms)
        {
            return TimeSpan.FromMilliseconds(ms).ToString(@"mm\:ss");
        }
    }
}
