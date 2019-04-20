using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Maps
{
    class MapFavourites : Script
    {
        public static void LoadPlayerFavourites(TDSPlayer player)
        {
            if (player.Entity == null)
                return;
            List<int> mapIDs = player.Entity.PlayerMapFavourites
                .Select(m => m.MapId)
                .ToList();
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.LoadMapFavourites, JsonConvert.SerializeObject(mapIDs));
        }

        [RemoteEvent(DToServerEvent.ToggleMapFavouriteState)]
        public static async void ToggleMapFavouriteState(Client player, string mapName, bool isFavourite)
        {
            Players? entity = player.GetEntity();
            if (entity == null)
                return;
            
            using (var dbcontext = new TDSNewContext())
            {
                int mapId = await dbcontext.Maps
                    .Where(m => m.Name == mapName)
                    .Select(m => m.Id)
                    .FirstOrDefaultAsync();
                if (mapId == 0)
                    return;

                PlayerMapFavourites favourite = await dbcontext.PlayerMapFavourites.FindAsync(entity.Id, mapId);

                #region Add Favourite
                if (favourite == null && isFavourite)
                {
                    favourite = new PlayerMapFavourites { Id = entity.Id, MapId = mapId };
                    dbcontext.PlayerMapFavourites.Add(favourite);
                    dbcontext.Entry(favourite).State = EntityState.Added;
                    await dbcontext.SaveChangesAsync();
                    return;
                }
                #endregion Add Favourite

                #region Remove Favourite
                if (favourite != null && !isFavourite)
                {
                    dbcontext.PlayerMapFavourites.Remove(favourite);
                    await dbcontext.SaveChangesAsync();
                    return;
                }
                #endregion Remove Favourite
            }
        }
    }
}
