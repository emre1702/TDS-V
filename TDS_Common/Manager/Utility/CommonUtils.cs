using NeoSmart.Hashing.XXHash;
using System;
using System.Text;

namespace TDS_Common.Manager.Utility
{
    public class CommonUtils
    {
        public static readonly Random Rnd = new Random();
        public static bool IsServersided { get; set; }

        public static string HashPWClient(string pw)
        {
            return XXHash64.Hash(Encoding.Default.GetBytes(pw)).ToString();
        }
    }
}