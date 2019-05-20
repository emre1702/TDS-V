using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;
using Z.EntityFramework.Plus;

namespace TDS_Server.Manager.Utility
{
    internal static class BansManager
    {
        public static async Task RemoveExpiredBans(TDSNewContext dbcontext)
        {
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            await Task.Delay(1);
            //todo add after z.Entity update
            /*await dbcontext.PlayerBans
                .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
                .DeleteAsync();
            await dbcontext.SaveChangesAsync();*/
            dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}