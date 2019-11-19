using GTANetworkAPI;
using System.Linq;
using Newtonsoft.Json;
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
        public static async void AddPlayerMapRating(Client player, int mapId, byte rating)
        {
            int? playerId = player.GetEntity()?.Id;
            if (playerId is null)
                return;

            MapDto? map = MapsLoader.GetMapById(mapId);
            bool isCustom = false; 
            if (map is null)
            {
                map = MapCreator.GetMapById(mapId);
                isCustom = true;
            }
            if (map is null)
                return;

            using var dbContext = new TDSDbContext();
            PlayerMapRatings? maprating = await dbContext.PlayerMapRatings.FindAsync(playerId, mapId);
            if (maprating is null)
            {
                maprating = new PlayerMapRatings { PlayerId = playerId.Value, MapId = mapId };
                dbContext.PlayerMapRatings.Add(maprating);
            }
            maprating.Rating = rating;
            map.SyncedData.Rating = rating;

            await dbContext.SaveChangesAsync();

            map.Ratings.Add(maprating);
            map.RatingAverage = map.Ratings.Average(r => r.Rating);

            if (isCustom)
                MapCreator.AddedMapRating(map);
        }

        public static void SendPlayerHisRatings(TDSPlayer character)
        {
            if (character.Entity is null)
                return;
            if (!character.Entity.PlayerMapRatings.Any())
                return;

            var ratingsDict = character.Entity.PlayerMapRatings.ToDictionary(r => r.MapId, r => r.Rating);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.LoadOwnMapRatings, JsonConvert.SerializeObject(ratingsDict));
        }
    }
}