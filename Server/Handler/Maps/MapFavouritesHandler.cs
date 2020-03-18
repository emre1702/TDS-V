﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Maps
{
    public class MapFavouritesHandler : DatabaseEntityWrapper
    {
        private readonly Serializer _serializer;

        public MapFavouritesHandler(EventsHandler eventsHandler, Serializer serializer, TDSDbContext dbContext, LoggingHandler loggingHandler) : base(dbContext, loggingHandler)
        {
            _serializer = serializer;

            eventsHandler.PlayerLoggedIn += LoadPlayerFavourites;
        }

        public async void LoadPlayerFavourites(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;

            List<int> mapIDs = await ExecuteForDBAsync(async dbContext
                => await dbContext.PlayerMapFavourites
                    .Where(m => m.PlayerId == player.Entity.Id)
                    .Select(m => m.MapId)
                    .ToListAsync());
            player.SendEvent(ToClientEvent.LoadMapFavourites, _serializer.ToBrowser(mapIDs));
        }

        public async void ToggleMapFavouriteState(ITDSPlayer player, int mapId, bool isFavorite)
        {

            await ExecuteForDBAsync(async dbContext =>
            {
                PlayerMapFavourites? favorite = await dbContext.PlayerMapFavourites.FindAsync(player.Id, mapId);

                #region Add Favourite

                if (favorite is null && isFavorite)
                {
                    favorite = new PlayerMapFavourites { PlayerId = player.Id, MapId = mapId };
                    dbContext.PlayerMapFavourites.Add(favorite);
                    await dbContext.SaveChangesAsync();
                    return;
                }

                #endregion Add Favourite

                #region Remove Favourite

                if (favorite != null && !isFavorite)
                {
                    dbContext.PlayerMapFavourites.Remove(favorite);
                    await dbContext.SaveChangesAsync();
                    return;
                }
                #endregion
            });

        }
    }
}