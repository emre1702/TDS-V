using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Models;
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
        public MapFavouritesHandler(EventsHandler eventsHandler, TDSDbContext dbContext, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
            : base(dbContext)
        {
            eventsHandler.PlayerLoggedIn += LoadPlayerFavourites;

            remoteBrowserEventsHandler.Add(ToServerEvent.ToggleMapFavouriteState, ToggleMapFavouriteState);
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

        public async Task<object?> ToggleMapFavouriteState(RemoteBrowserEventArgs args)
        {
            int mapId = (int)args.Args[0];
            bool isFavorite = (bool)args.Args[1];

            await ExecuteForDBAsync(async dbContext =>
            {
                PlayerMapFavourites? favorite = await dbContext.PlayerMapFavourites.FindAsync(args.Player.Id, mapId).ConfigureAwait(false);

                if (favorite is null && isFavorite)
                    await AddMapFavorite(args.Player, mapId, dbContext).ConfigureAwait(false);
                else if (favorite is { } && !isFavorite)
                    await RemoveMapFavorite(favorite, dbContext);
            }).ConfigureAwait(false);
            return null;
        }

        private Task AddMapFavorite(ITDSPlayer player, int mapId, TDSDbContext dbContext)
        {
            var favorite = new PlayerMapFavourites { PlayerId = player.Id, MapId = mapId };
            dbContext.PlayerMapFavourites.Add(favorite);
            return dbContext.SaveChangesAsync();
        }

        private Task RemoveMapFavorite(PlayerMapFavourites favorite, TDSDbContext dbContext)
        {
            dbContext.PlayerMapFavourites.Remove(favorite);
            return dbContext.SaveChangesAsync();
        }
    }
}