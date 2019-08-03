using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Maps
{
    internal static class MapsRatings
    {
        public static TDSNewContext DbContext { get; set; }

        static MapsRatings()
        {
            DbContext = new TDSNewContext();
        }

        public static async void AddPlayerMapRating(Client player, int mapId, byte rating)
        {
            int? playerId = player.GetEntity()?.Id;
            if (playerId == null)
                return;

            MapDto? map = MapsLoader.GetMapById(mapId);
            bool isCustom = false; 
            if (map == null)
            {
                map = MapCreator.GetMapById(mapId);
                isCustom = true;
            }
            if (map == null)
                return;

            PlayerMapRatings? maprating = await DbContext.PlayerMapRatings.FindAsync(playerId, mapId);
            if (maprating == null)
            {
                maprating = new PlayerMapRatings { PlayerId = playerId.Value, MapId = mapId };
                DbContext.PlayerMapRatings.Add(maprating);
            }
            maprating.Rating = rating;
            map.SyncedData.Rating = rating;
            await DbContext.SaveChangesAsync();

            if (isCustom)
                MapCreator.AddedMapRating(map);
        }

        public static void SendPlayerHisRatings(TDSPlayer character)
        {
            if (character.Entity == null)
                return;
            if (!character.Entity.PlayerMapRatings.Any())
                return;

            var ratingsDict = character.Entity.PlayerMapRatings.ToDictionary(r => r.MapId, r => r.Rating);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.LoadOwnMapRatings, JsonConvert.SerializeObject(ratingsDict));
        }
    }
}