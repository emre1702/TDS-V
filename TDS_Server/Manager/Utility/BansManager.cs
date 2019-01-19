using TDS_Server.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TDS_Server.Manager.Utility
{
    static class BansManager
    {

        public static void RemoveExpiredBans(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(
                dbcontext.PlayerBans
                    .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
#warning  todo: Check if that works right
                    .ToList()
            );
            dbcontext.SaveChanges();
        }
    }
}
