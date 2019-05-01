using TDS_Server.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TDS_Server.Manager.Utility
{
    static class BansManager
    {

        public static async Task RemoveExpiredBans(TDSNewContext dbcontext)
        {
            dbcontext.RemoveRange(
                await dbcontext.PlayerBans
                    .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
//todo  todo: Check if that works right
                    .ToListAsync()
            );
            await dbcontext.SaveChangesAsync();
        }
    }
}
