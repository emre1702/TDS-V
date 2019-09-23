using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TDS_Common.Dto.Map;
using TDS_Server.Dto.Map;

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
            using var sha512 = SHA512.Create();
            byte[] hashbytes = sha512.ComputeHash(Encoding.Default.GetBytes(pw));
            using var sha384 = SHA384.Create();
            hashbytes = sha384.ComputeHash(hashbytes);
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

        public static string GetReplaced(string str, params object[] args)
        {
            if (args.Length > 0)
            {
                _strbuilder.Append(str);
                for (int i = 0; i < args.Length; ++i)
                {
                    _strbuilder.Replace("{" + i + "}", args[i] is null ? "?" : args[i].ToString());
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

        public static Vector3 ToVector3(this Position3DDto pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
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