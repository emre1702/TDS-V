using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Database;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Database
{
    public class BaseLobbyDatabase : IBaseLobbyDatabase
    {
        protected IDatabaseHandler DbHandler { get; }
        protected IBaseLobby Lobby { get; }
        protected IBaseLobbyEventsHandler Events { get; }

        public BaseLobbyDatabase(IBaseLobby lobby, IDatabaseHandler dbHandler, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            DbHandler = dbHandler;
            Events = events;

            DbHandler.ExecuteForDBWithoutWait(dbContext => dbContext.Attach(lobby.Entity));

            events.Created += AddCreatedDatabaseToDatabase;
            events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            if (Events.Created is { })
                Events.Created -= AddCreatedDatabaseToDatabase;
            Events.RemoveAfter -= RemoveEvents;
        }

        private async Task AddCreatedDatabaseToDatabase(LobbyDb entity)
        {
            if (entity.Id != 0)
                return;

            await DbHandler.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                await dbContext.Entry(entity)
                    .Reference(e => e.Owner)
                    .LoadAsync()
                    .ConfigureAwait(false);

                await dbContext.Entry(entity)
                    .Collection(e => e.LobbyMaps)
                    .Query()
                    .Include(e => e.Map)
                    .LoadAsync()
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        public async Task<PlayerBans?> GetBan(int? playerId)
        {
            if (!playerId.HasValue)
                return null;
            return await DbHandler
                .ExecuteForDBAsync(async (dbContext)
                    => await dbContext.PlayerBans.FindAsync(playerId, Lobby.Entity.Id)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public Task<string?> GetLastUsedSerial(int playerId)
        {
            return DbHandler.ExecuteForDBAsync(async (dbContext)
                => (string?)await dbContext.LogRests
                                        .Where(l => l.Source == playerId)
                                        .OrderByDescending(l => l.Id)
                                        .Select(l => l.Serial)
                                        .FirstOrDefaultAsync()
                                        .ConfigureAwait(false));
        }

        public Task AddBanEntity(PlayerBans ban)
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.PlayerBans.Add(ban);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            });
        }

        public Task Remove<T>(object obj)
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Remove(obj);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            });
        }

        public Task Save()
        {
            return DbHandler.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            });
        }
    }
}
