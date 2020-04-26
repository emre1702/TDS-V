using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Shared.Default;
using TDS_Shared.Core;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.Handler.Maps
{
    public class MapFavouritesHandler : DatabaseEntityWrapper
    {
        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;

        public MapFavouritesHandler(EventsHandler eventsHandler, Serializer serializer, TDSDbContext dbContext, ILoggingHandler loggingHandler, IModAPI modAPI) 
            : base(dbContext, loggingHandler)
        {
            _modAPI = modAPI;
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
            _modAPI.Thread.RunInMainThread(() => player.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadMapFavourites, _serializer.ToBrowser(mapIDs)));
        }

        public async Task<object?> ToggleMapFavouriteState(ITDSPlayer player, object[] args)
        {
            int mapId = (int)args[0];
            bool isFavorite = (bool)args[1];

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
            return null;

        }
    }
}
