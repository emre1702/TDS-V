using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler.Helper
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

                foreach (var entry in dbContext.ChangeTracker.Entries())
                    entry.State = EntityState.Detached;
            }).Wait();
        }

        public async Task ChangePlayerMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                _chatHandler.OutputMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;

            await Save(target.PlayerStats).ConfigureAwait(false);
        }

        public async Task ChangePlayerVoiceMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;

            await Save(target.PlayerStats).ConfigureAwait(false);

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

        public async Task<bool> DoesPlayerWithScnameExist(string scName)
        {
            int id = await GetPlayerIDByScname(scName).ConfigureAwait(false);
            return id is { } && id != 0;
        }

        public async Task<Players?> GetPlayerByName(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                    await dbContext.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()).ConfigureAwait(false))
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

        internal async Task<DatabasePlayerIdName?> GetPlayerIdName(ITDSPlayer player)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players.Where(p => p.Name == player.Name || p.SCName == player.SocialClubName)
                    .AsNoTracking()
                    .Select(p => new DatabasePlayerIdName(p.Id, p.Name))
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task Save<TEntity>(TEntity entity) where TEntity : class
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                dbContext.Entry(entity).State = EntityState.Detached;
            }).ConfigureAwait(false);
        }
    }
}
