using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Core.Manager.Maps
{
    internal class MapFavourites : Script
    {
        public static void LoadPlayerFavourites(TDSPlayer player)
        {
            if (player.Entity is null)
                return;
            using var dbContext = new TDSDbContext();
            List<int> mapIDs = dbContext.PlayerMapFavourites
                .Where(m => m.PlayerId == player.Entity.Id)
                .Select(m => m.MapId)
                .ToList();
            NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.LoadMapFavourites, Serializer.ToBrowser(mapIDs));
        }

        [RemoteEvent(DToServerEvent.ToggleMapFavouriteState)]
        public static async void ToggleMapFavouriteState(Player player, int mapId, bool isFavorite)
        {
            Players? entity = player.GetEntity();
            if (entity is null)
                return;

            using var dbContext = new TDSDbContext();
            PlayerMapFavourites? favorite = await dbContext.PlayerMapFavourites.FindAsync(entity.Id, mapId);

            #region Add Favourite

            if (favorite is null && isFavorite)
            {
                favorite = new PlayerMapFavourites { PlayerId = entity.Id, MapId = mapId };
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
        }
    }
}