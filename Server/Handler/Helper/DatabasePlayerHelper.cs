﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.Helper
{
    public class DatabasePlayerHelper : DatabaseEntityWrapper
    {
        private readonly ChatHandler _chatHandler;

        public DatabasePlayerHelper(TDSDbContext dbContext, ChatHandler chatHandler)
            : base(dbContext)
        {
            _chatHandler = chatHandler;

            ExecuteForDB(dbContext =>
            {
                var playerStats = dbContext.PlayerStats.Where(p => p.LoggedIn).ToList();
                foreach (var stat in playerStats)
                {
                    stat.LoggedIn = false;
                }
                dbContext.SaveChanges();
            }).Wait();
        }

        public async Task ChangePlayerMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                _chatHandler.OutputMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(target.PlayerStats).State = EntityState.Modified;

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                dbContext.Entry(target.PlayerStats).State = EntityState.Detached;
            }).ConfigureAwait(false);
        }

        public async Task ChangePlayerVoiceMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(target.PlayerStats).State = EntityState.Modified;

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                dbContext.Entry(target.PlayerStats).State = EntityState.Detached;
            }).ConfigureAwait(false);
        }

        public async Task<bool> DoesPlayerWithNameExist(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                 await dbContext.Players
                    .AsNoTracking()
                    .AnyAsync(p => EF.Functions.ILike(p.Name, name))
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<bool> DoesPlayerWithScnameExist(string scname)
        {
            int id = await GetPlayerIDByScname(scname).ConfigureAwait(false);
            return id is { } && id != 0;
        }

        public async Task<Players?> GetPlayerByName(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                    await dbContext.Players.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<int> GetPlayerIDByName(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .AsNoTracking()
                    .Where(p => p.Name == name)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<int> GetPlayerIDByScname(string scname)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .AsNoTracking()
                    .Where(p => p.SCName == scname)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        internal async Task<DatabasePlayerIdName?> GetPlayerIdName(ITDSPlayer modPlayer)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players.Where(p => p.Name == modPlayer.Name || p.SCName == modPlayer.SocialClubName)
                    .Select(p => new DatabasePlayerIdName(p.Id, p.Name))
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }
}
