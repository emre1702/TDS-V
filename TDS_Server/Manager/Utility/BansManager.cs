using TDS_Server.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace TDS_Server.Manager.Utility
{
    static class BansManager
    {

        public static async void RemoveExpiredBans(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(
                await dbcontext.Playerbans
                    .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
#warning  todo: Check if that works right
                    .ToListAsync()
            );
        }
    }
}
