using System;
using System.Linq;
using System.Security.Claims;

namespace TDSCPServer.Utils
{
    public class User
    {

        public static uint GetUID(ClaimsPrincipal user)
        {
            string uidstr = user.Claims.FirstOrDefault(c => c.Type == "UID").Value;
            if (uidstr == null)
                return 0;
            return Convert.ToUInt32(uidstr);
        }
    }
}
