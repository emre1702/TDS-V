using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TDS_Server.Manager.Utility
{
    internal static class Utils
    {
        private static readonly StringBuilder _strbuilder = new StringBuilder();
        private static readonly DateTime _startDateTime = new DateTime(2017, 7, 24);

        public static uint GetTimespanSinceStart(int seconds = 0)
        {
            TimeSpan t = DateTime.Now.AddSeconds(seconds) - _startDateTime;
            return (uint)t.TotalSeconds;
        }

        public static string GetTimestamp()
        {
            return DateTime.Now.ToString("s");
        }

        public static string HashPWServer(string pw)
        {
            byte[] hashbytes = SHA512.Create().ComputeHash(Encoding.Default.GetBytes(pw));
            hashbytes = SHA384.Create().ComputeHash(hashbytes);
            for (int i = 0; hashbytes != null && i < hashbytes.Length; i++)
            {
                _strbuilder.AppendFormat("{0:x2}", hashbytes[i]);
            }
            string result = _strbuilder.ToString();
            _strbuilder.Clear();
            return result;
        }

        public static float ToFloat(this string str)
        {
            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        public static Client? FindPlayer(string name)
        {
            Client? player = NAPI.Player.GetPlayerFromName(name);
            if (player != null && player.Exists)
                return player;
            name = name.ToLower();
            return NAPI.Pools.GetAllPlayers().FirstOrDefault(c => c.Name.ToLower().StartsWith(name));
        }

        public static string GetReplaced(string str, params object[] args)
        {
            if (args.Length > 0)
            {
                _strbuilder.Append(str);
                for (int i = 0; i < args.Length; ++i)
                {
                    _strbuilder.Replace("{" + i + "}", args[i] == null ? "?" : args[i].ToString());
                }
                string result = _strbuilder.ToString();
                _strbuilder.Clear();
                return result;
            }
            return str;
        }

        public static string Formatted(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string DurationTo(this DateTime time1, DateTime time2)
        {
            TimeSpan span = time2 - time1;
            return $"{(int)(span.TotalMinutes / 60)}:{(int)Math.Ceiling(span.TotalMinutes % 60)}";
        }

        public static uint? GetVehicleFreeSeat(Vehicle veh)
        {
            HashSet<int> occupiedSeats = veh.Occupants.Select(o => o.VehicleSeat).ToHashSet();
            for (int i = veh.MaxOccupants - 1; i >= 0; --i)
            {
                if (!occupiedSeats.Contains(i))
                    return (uint?)i;
            }
            return null;
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, "_");
        }
    }
}