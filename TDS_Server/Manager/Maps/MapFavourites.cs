using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Maps
{
    internal class MapFavourites : Script
    {
        public static void LoadPlayerFavourites(TDSPlayer player)
        {
            if (player.Entity == null)
                return;
            using var dbContext = new TDSNewContext();
            List<int> mapIDs = dbContext.PlayerMapFavourites
                .Where(m => m.PlayerId == player.Entity.Id)
                .Select(m => m.MapId)
                .ToList();
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LoadMapFavourites, JsonConvert.SerializeObject(mapIDs));
        }

        [RemoteEvent(DToServerEvent.ToggleMapFavouriteState)]
        public static async void ToggleMapFavouriteState(Client player, int mapId, bool isFavorite)
        {
            Players? entity = player.GetEntity();
            if (entity == null)
                return;

            using var dbContext = new TDSNewContext();
            PlayerMapFavourites? favorite = await dbContext.PlayerMapFavourites.FindAsync(entity.Id, mapId);

            #region Add Favourite

            if (favorite == null && isFavorite)
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