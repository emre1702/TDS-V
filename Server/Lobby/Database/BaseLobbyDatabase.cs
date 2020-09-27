using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDS_Server.Data.Interfaces.LobbySystem.Database;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Database
{
    public class BaseLobbyDatabase : IBaseLobbyDatabase
    {
        protected DatabaseHandler DbHandler { get; }
        protected readonly IBaseLobby Lobby;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobbyDatabase(IBaseLobby lobby, DatabaseHandler dbHandler, IBaseLobbyEventsHandler events)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Lobby = lobby;
            DbHandler = dbHandler;

            DbHandler.ExecuteForDBWithoutWait(dbContext => dbContext.Attach(lobby.Entity));

            events.Created += AddCreatedDatabaseToDatabase;
        }

        private async Task AddCreatedDatabaseToDatabase(LobbyDb entity)
        {
            await DbHandler.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(entity)
                    .Reference(e => e.Owner)
                    .LoadAsync();

                await dbContext.Entry(entity)
                    .Collection(e => e.LobbyMaps)
                    .Query()
                    .Include(e => e.Map)
                    .LoadAsync();
            });
        }

        public async Task<PlayerBans?> GetBan(int? playerId)
        {
            if (!playerId.HasValue)
                return null;
            return await DbHandler.ExecuteForDBAsync(async (dbContext) => await dbContext.PlayerBans.FindAsync(playerId, Lobby.Entity.Id));
        }

        public Task<string?> GetLastUsedSerial(int playerId)
        {
            return DbHandler.ExecuteForDBAsync(async (dbContext)
                => (string?)await dbContext.LogRests
                                        .Where(l => l.Source == playerId)
                                        .OrderByDescending(l => l.Id)
                                        .Select(l => l.Serial)
                                        .FirstOrDefaultAsync());
        }

        public Task AddBanEntity(PlayerBans ban)
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.PlayerBans.Add(ban);
                await dbContext.SaveChangesAsync();
            });
        }

        public Task Remove<T>(object obj)
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(obj);
                await dbContext.SaveChangesAsync();
            });
        }

        public Task Save()
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
