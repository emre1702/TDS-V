﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;

namespace TDS_Server.Data
{
    public static class Utils
    {
#nullable enable
        public static string GetTimestamp()
        {
            return DateTime.UtcNow.ToString("s");
        }

        public static string GetUniversalDateTimeString(DateTime dateTime)
        {
            var enUsCulture = CultureInfo.CreateSpecificCulture("en-US");
            return new DateTimeOffset(dateTime).ToString("f", enUsCulture) + " +00:00";
        }

        public static string HashPWServer(string pw)
        {
            var stringBuilder = new StringBuilder();
            using var sha512 = SHA512.Create();
            byte[] hashbytes = sha512.ComputeHash(Encoding.Default.GetBytes(pw));
            using var sha384 = SHA384.Create();
            hashbytes = sha384.ComputeHash(hashbytes);
            for (int i = 0; hashbytes != null && i < hashbytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", hashbytes[i]);
            }
            string result = stringBuilder.ToString();
            return result;
        }

        public static float ToFloat(this string str)
        {
            return float.Parse(str, CultureInfo.InvariantCulture);
        }

        public static string GetReplaced(string str, params object[] args)
        {
            var stringBuilder = new StringBuilder();
            if (args.Length > 0)
            {
                stringBuilder.Append(str);
                for (int i = 0; i < args.Length; ++i)
                {
                    stringBuilder.Replace("{" + i + "}", args[i] is null ? "?" : args[i].ToString());
                }
                string result = stringBuilder.ToString();
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

        public static uint? GetVehicleFreeSeat(IVehicle veh)
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

        public static int? GetInt(object obj)
        {
            if (int.TryParse(Convert.ToString(obj), out int result))
                return result;
            return null;
        }

        public static bool GetInt(object obj, out int id)
        {
            return int.TryParse(Convert.ToString(obj), out id);
        }

        public static IEnumerable<string> SplitByLength(string str, int length)
            => Enumerable
                    .Range(0, str.Length / length)
                    .Select(i => str.Substring(i * length, length));

        public static List<string> SplitPartsByLength(string str, int length, string partLimiter = "\n")
        {
            var list = new List<string>();
            var splitted = str.Split(partLimiter, StringSplitOptions.RemoveEmptyEntries);

            string currentStr = string.Empty;
            for (int i = 0; i < splitted.Length; ++i)
            {
                var part = splitted[i];
                if (currentStr.Length + part.Length + partLimiter.Length <= length)
                {
                    if (currentStr.Length > 0)
                        currentStr += partLimiter;
                    currentStr += part;
                }
                else if (currentStr.Length == 0)
                {
                    var strList = SplitByLength(currentStr, length);
                    for (int j = 0; j < strList.Count() - 1; ++j)
                    {
                        list.Add(strList.ElementAt(j));
                    }
                    currentStr = strList.Last();
                }
                else
                {
                    list.Add(currentStr);
                    currentStr = part;
                }
            }
            if (currentStr.Length > 0)
                list.Add(currentStr);

            return list;

        }


        public static uint GetMsToNextHour()
        {
            var timeOfDay = DateTime.UtcNow.TimeOfDay;
            var nextFullHour = TimeSpan.FromHours(Math.Ceiling(timeOfDay.TotalHours));

            return (uint)(nextFullHour - timeOfDay).TotalMilliseconds + 1;
        }

        public static uint GetMsToNextMinute()
        {
            var timeOfDay = DateTime.UtcNow.TimeOfDay;
            var nextFullMinute = TimeSpan.FromMinutes(Math.Ceiling(timeOfDay.TotalMinutes));

            return (uint)(nextFullMinute - timeOfDay).TotalMilliseconds + 1;
        }

        public static uint GetMsToNextSecond()
        {
            var timeOfDay = DateTime.UtcNow.TimeOfDay;
            var nextFullSecond = TimeSpan.FromSeconds(Math.Ceiling(timeOfDay.TotalSeconds));

            return (uint)(nextFullSecond - timeOfDay).TotalMilliseconds + 1;
        }

        public static bool HandleBan(IPlayer modPlayer, PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
            //todo Test line break and display

            var splittedReason = Utils.SplitPartsByLength($"Banned!\nName: {ban.Player?.Name ?? modPlayer.Name}\nAdmin: {ban.Admin.Name}\nReason: {ban.Reason}\nEnd: {endstr} UTC\nStart: {startstr} UTC", 90);
            foreach (var split in splittedReason)
                modPlayer.SendNotification(split, true);

            _ = new TDSTimer(() =>
            {
                if (!modPlayer.IsNull)
                    modPlayer.Kick("Ban");
            }, 3000, 1);


            return false;
        }

        /// <summary>
        /// Check if the name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The one char being not valid.</returns>
        public static char? CheckNameValid(string name)
        {
            byte min = System.Enum.GetValues(typeof(CharASCII)).Cast<byte>().Min();
            byte max = System.Enum.GetValues(typeof(CharASCII)).Cast<byte>().Max();
            foreach (byte number in Encoding.ASCII.GetBytes(name))
            {
                if (number < min || number > max)
                    return (char)number;
                if (number == (byte)CharASCII.AtSign
                        || number == (byte)CharASCII.GraveAccent
                        || number == (byte)CharASCII.Apostrophe
                        || number == (byte)CharASCII.LeftCurlyBracket
                        || number == (byte)CharASCII.RightCurlyBracket)
                    return (char)number;

            }
            return null;
        }


        private enum CharASCII : byte
        {
            Space = 32,
            ExclamationMark = 33,
            QuoteDouble = 34,
            Route = 35,
            Dollar = 36,
            Percent = 37,
            And = 38,
            Apostrophe = 39,
            LeftParenthesis = 40,
            RightParenthesis = 41,
            Asterisk = 42,
            Plus = 43,
            Comma = 44,
            Minus = 45,
            Dot = 46,
            Slash = 47,
            Digit0 = 48,
            // ...
            Digit9 = 57,
            Colon = 58,
            Semicolon = 59,
            LessThan = 60,
            Equal = 61,
            GreaterThan = 62,
            QuestionMark = 63,
            AtSign = 64,
            CharA = 65,
            // ...
            CharZ = 90,
            LeftSquareBracket = 91,
            BackSlash = 92,
            RightSquareBracket = 93,
            Circumflex = 94,                    // ^
            LowLine = 95,                       // _
            GraveAccent = 96,                   // `
            SmallCharA = 97,
            // ...
            SmallCharZ = 122,
            LeftCurlyBracket = 123,
            VerticalBar = 124,
            RightCurlyBracket = 125,
            Tilde = 126

        }
    }
}
