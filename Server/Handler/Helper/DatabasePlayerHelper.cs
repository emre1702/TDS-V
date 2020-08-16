using AltV.Net.Async;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;

namespace TDS_Server.Core.Manager.PlayerManager
{
    public class DatabasePlayerHelper : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly ChatHandler _chatHandler;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePlayerHelper(TDSDbContext dbContext, ILoggingHandler loggingHandler, ChatHandler chatHandler)
            : base(dbContext, loggingHandler)
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

        #endregion Public Constructors

        #region Public Methods

        public async Task ChangePlayerMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            await AltAsync.Do(() =>
            {
                _chatHandler.OutputMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.MuteTime = minutes == -1 ? (int?)null : minutes;

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(target.PlayerStats).State = EntityState.Modified;

                await dbContext.SaveChangesAsync();
                dbContext.Entry(target.PlayerStats).State = EntityState.Detached;
            });
        }

        public async Task ChangePlayerVoiceMuteTime(ITDSPlayer admin, Players target, int minutes, string reason)
        {
            await AltAsync.Do(() =>
            {
                _chatHandler.OutputVoiceMuteInfo(admin.DisplayName, target.Name, minutes, reason);
            });

            target.PlayerStats.VoiceMuteTime = minutes == -1 ? (int?)null : minutes;

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(target.PlayerStats).State = EntityState.Modified;

                await dbContext.SaveChangesAsync();
                dbContext.Entry(target.PlayerStats).State = EntityState.Detached;
            });
        }

        public async Task<bool> DoesPlayerWithNameExist(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                 await dbContext.Players
                    .AsNoTracking()
                    .AnyAsync(p => EF.Functions.ILike(p.Name, name))
                    .ConfigureAwait(false));
        }

        public async Task<bool> DoesPlayerWithScIdExist(ulong scId)
        {
            int id = await GetPlayerIDByScId(scId).ConfigureAwait(false);
            return id is { } && id != 0;
        }

        public async Task<Players?> GetPlayerByName(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower()));
        }

        public async Task<int> GetPlayerIDByName(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .AsNoTracking()
                    .Where(p => p.Name == name)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false));
        }

        public async Task<int> GetPlayerIDByScId(ulong scId)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .AsNoTracking()
                    .Where(p => p.SCId == scId)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false));
        }

        #endregion Public Methods

        #region Internal Methods

        internal async Task<DatabasePlayerIdName?> GetPlayerIdName(ITDSPlayer player)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players.Where(p => p.Name == player.Name || p.SCId == player.SocialClubId)
                    .Select(p => new DatabasePlayerIdName(p.Id, p.Name))
                    .FirstOrDefaultAsync());
        }

        #endregion Internal Methods
    }
}
