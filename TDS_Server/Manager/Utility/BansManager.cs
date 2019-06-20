﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;
using Z.EntityFramework.Plus;

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
            await DbContext.PlayerBans
                .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.Now)
                .DeleteAsync();
        }
    }
}