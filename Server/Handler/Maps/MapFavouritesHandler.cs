﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Maps
{
    public class MapFavouritesHandler : DatabaseEntityWrapper
    {
        public MapFavouritesHandler(EventsHandler eventsHandler, TDSDbContext dbContext)
            : base(dbContext)
        {
            eventsHandler.PlayerLoggedIn += LoadPlayerFavourites;
        }

        public async void LoadPlayerFavourites(ITDSPlayer player)
        {
            try
            {
                if (player.Entity is null)
                    return;

                var mapIDs = await ExecuteForDBAsync(async dbContext
                    => await dbContext.PlayerMapFavourites
                        .Where(m => m.PlayerId == player.Entity.Id)
                        .Select(m => m.MapId)
                        .ToListAsync().ConfigureAwait(false))
                    .ConfigureAwait(false);
                var json = Serializer.ToBrowser(mapIDs);
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadMapFavourites, json));
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async Task<object?> ToggleMapFavouriteState(ITDSPlayer player, ArraySegment<object> args)
        {
            int mapId = (int)args[0];
            bool isFavorite = (bool)args[1];

            await ExecuteForDBAsync(async dbContext =>
            {
                PlayerMapFavourites? favorite = await dbContext.PlayerMapFavourites.FindAsync(player.Id, mapId).ConfigureAwait(false);

                #region Add Favourite

                if (favorite is null && isFavorite)
                {
                    favorite = new PlayerMapFavourites { PlayerId = player.Id, MapId = mapId };
                    dbContext.PlayerMapFavourites.Add(favorite);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    return;
                }

                #endregion Add Favourite

                #region Remove Favourite

                if (favorite != null && !isFavorite)
                {
                    dbContext.PlayerMapFavourites.Remove(favorite);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    return;
                }

                #endregion Remove Favourite
            }).ConfigureAwait(false);
            return null;
        }
    }
}
