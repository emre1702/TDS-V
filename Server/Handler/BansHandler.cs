﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;

namespace TDS_Server.Handler
{
    public class BansHandler : DatabaseEntityWrapper
    {
        public BansHandler(TDSDbContext dbContext, LoggingHandler logger) : base(dbContext, logger) { }

        public async Task<PlayerBans?> GetBan(int lobbyId,
            int? playerId = null, string? ip = null, string? serial = null, string? socialClubName = null, ulong? socialClubId = null,
            bool? preventConnection = null, bool andConnection = false)
        {
            PlayerBans? ban = (playerId, ip, serial, socialClubName, socialClubId, andConnection) switch
            {
                ({ }, null, null, null, null, _) 
                    => await ExecuteForDBAsync(async (dbContext) =>
                        {
                            return await dbContext.PlayerBans.FindAsync(playerId, lobbyId);
                        }),

                (_, _, _, _, _, true) 
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId
                                && (playerId == null || b.PlayerId == playerId)
                                && (ip == null || b.IP == ip)
                                && (serial == null || b.Serial == serial)
                                && (socialClubName == null || b.SCName == socialClubName)
                                && (socialClubId == null || b.SCId == socialClubId)
                                && (preventConnection == null || b.PreventConnection == preventConnection))
                            .FirstOrDefaultAsync();
                    }),

                (_, _, _, _, _, false)
                    => await ExecuteForDBAsync(async (dbContext) =>
                    {
                        return await dbContext.PlayerBans
                            .Where(b => b.LobbyId == lobbyId && (
                                (playerId == null || b.PlayerId == playerId)
                                || (ip == null || b.IP == ip)
                                || (serial == null || b.Serial == serial)
                                || (socialClubName == null || b.SCName == socialClubName)
                                || (socialClubId == null || b.SCId == socialClubId)
                                || (preventConnection == null || b.PreventConnection == preventConnection)))
                            .FirstOrDefaultAsync();
                    }),
            };

            if (ban is null)
                return null;

            if (ban.EndTimestamp < DateTime.UtcNow)
            {
                await ExecuteForDBAsync(async (dbContext) =>
                {
                    dbContext.Entry(ban).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                });
                return null;
            }

            return ban;
        }

        public async Task RemoveExpiredBans()
        {
            await ExecuteForDBAsync(async (dbContext) =>
            {
                var bans = await dbContext.PlayerBans
                    .Where(b => b.EndTimestamp.HasValue && b.EndTimestamp.Value < DateTime.UtcNow)
                    .ToListAsync();
                dbContext.PlayerBans.RemoveRange(bans);
                await dbContext.SaveChangesAsync();
            });

        }
    }
}