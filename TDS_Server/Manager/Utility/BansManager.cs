using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal static class BansManager
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