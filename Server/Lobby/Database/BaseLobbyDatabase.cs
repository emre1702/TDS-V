﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Handler;
using TDS_Server.LobbySystem.Lobbies;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Database
{
    public class BaseLobbyDatabase
    {
        protected DatabaseHandler DbHandler { get; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public BaseLobbyDatabase(BaseLobby lobby, DatabaseHandler dbHandler, IBaseLobbyEventsHandler eventsHandler)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            DbHandler = dbHandler;

            DbHandler.ExecuteForDBWithoutWait(dbContext => dbContext.Attach(lobby.Entity));

            eventsHandler.LobbyCreated += AddCreatedToDatabase;
        }

        private async Task AddCreatedToDatabase(LobbyDb entity)
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
    }
}
