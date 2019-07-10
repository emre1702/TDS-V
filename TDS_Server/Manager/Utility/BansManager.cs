using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal static class BansManager
    {
        public static TDSNewContext DbContext { get; set; }

        static BansManager()
        {
            DbContext = new TDSNewContext();
        }

        public static async Task RemoveExpiredBans()
        {
            var bans = await DbContext.PlayerBans
                .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
                .ToListAsync();
            DbContext.PlayerBans.RemoveRange(bans);
            await DbContext.SaveChangesAsync();
        }
    }
}