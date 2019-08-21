using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Utility
{
    internal static class BansManager
    {
        public static async Task RemoveExpiredBans()
        {
            using var dbContext = new TDSNewContext();
            var bans = await dbContext.PlayerBans
                .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
                .ToListAsync();
            dbContext.PlayerBans.RemoveRange(bans);
            await dbContext.SaveChangesAsync();
        }
    }
}