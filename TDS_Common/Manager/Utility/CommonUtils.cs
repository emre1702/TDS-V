using NeoSmart.Hashing.XXHash;
using System.Text;

namespace TDS_Common.Manager.Utility
{
    public class CommonUtils
    {
        public static string HashPWClient(string pw)
        {
            return XXHash64.Hash(Encoding.Default.GetBytes(pw)).ToString();
        }
    }
}
