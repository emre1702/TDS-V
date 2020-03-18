using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;

namespace TDS_Server.Core.Manager.PlayerManager
{
    public class DatabasePlayerHelper : DatabaseEntityWrapper
    {
        public DatabasePlayerHelper(TDSDbContext dbContext, LoggingHandler loggingHandler) : base(dbContext, loggingHandler)
        {
            ExecuteForDB(dbContext =>
                dbContext.PlayerStats.Where(p => p.LoggedIn).UpdateFromQuery(p => new PlayerStats { LoggedIn = false })).RunSynchronously();
        }


        public async Task<bool> DoesPlayerWithScnameExist(string scname)
        {
            return await GetPlayerIDByScname(scname).ConfigureAwait(false) != 0;
        }

        public async Task<bool> DoesPlayerWithNameExist(string name)
        {
            return await ExecuteForDBAsync(async dbContext =>
                 await dbContext.Players
                    .AsNoTracking()
                    .AnyAsync(p => EF.Functions.ILike(p.Name, name))
                    .ConfigureAwait(false));
        }

        public async Task<int> GetPlayerIDByScname(string scname)
        {
            return await ExecuteForDBAsync(async dbContext =>
                await dbContext.Players
                    .AsNoTracking()
                    .Where(p => p.SCName == scname)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false));
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
    }
}
