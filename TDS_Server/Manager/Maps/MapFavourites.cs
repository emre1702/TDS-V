using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Maps
{
    internal class MapFavourites : Script
    {
        public static TDSNewContext DbContext { get; set; }

        static MapFavourites()
        {
            DbContext = new TDSNewContext();
        }

        public static void LoadPlayerFavourites(TDSPlayer player)
        {
            if (player.Entity == null)
                return;
            List<int> mapIDs = DbContext.PlayerMapFavourites
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

            PlayerMapFavourites? favorite = await DbContext.PlayerMapFavourites.FindAsync(entity.Id, mapId);

            #region Add Favourite

            if (favorite == null && isFavorite)
            {
                favorite = new PlayerMapFavourites { PlayerId = entity.Id, MapId = mapId };
                DbContext.PlayerMapFavourites.Add(favorite);
                await DbContext.SaveChangesAsync();
                return;
            }

            #endregion Add Favourite

            #region Remove Favourite

            if (favorite != null && !isFavorite)
            {
                DbContext.PlayerMapFavourites.Remove(favorite);
                await DbContext.SaveChangesAsync();
                return;
            }
            #endregion
        }
    }
}