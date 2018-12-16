namespace TDS.Manager.Utility
{
    using GTANetworkAPI;
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    static class Utils
    {
        private static readonly StringBuilder strbuilder = new StringBuilder();
        public static readonly Random Rnd = new Random();
        private static readonly DateTime startDateTime = new DateTime(2017, 7, 24);

        public static uint GetTimespan(int seconds = 0)
        {
            TimeSpan t = DateTime.Now.AddSeconds(seconds) - startDateTime;
            return (uint)t.TotalSeconds;
        }

        public static string GetTimestamp(int seconds = 0)
        {
            if (seconds == 0)
                return DateTime.UtcNow.AddHours(2).ToString("dd-MM-yyyy HH:mm:ss");
            return DateTime.UtcNow.AddHours(2).AddSeconds(seconds).ToString("dd-MM-yyyy HH:mm:ss");
        }

        public static string ToSHA512(string input)
        {
            byte[] hashbytes = SHA512.Create().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; hashbytes != null && i < hashbytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", hashbytes[i]);
            }
            return sb.ToString();
        }

        public static float ToFloat(this string str)
        {
            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        public static Client FindPlayer(string name)
        {
            Client player = NAPI.Player.GetPlayerFromName(name);
            if (player.Exists)
                return player;
            name = name.ToLower();
            return NAPI.Pools.GetAllPlayers().Find(c => c.Name.ToLower().StartsWith(name));
        }

        public static string GetReplaced(string str, params object[] args)
        {
            if (args.Length > 0)
            {
                strbuilder.Append(str);
                for (int i = 0; i < args.Length; ++i)
                {
                    strbuilder.Replace("{" + (i + 1) + "}", args[i].ToString());
                }
                string result = strbuilder.ToString();
                strbuilder.Clear();
                return result;
            }
            return str;
        }

        public static string DurationTo(this DateTime time1, DateTime time2)
        {
            TimeSpan span = time2 - time1;
            return $"{(int)(span.TotalMinutes / 60)}:{(int)Math.Ceiling(span.TotalMinutes % 60)}";
        }

    }

}
