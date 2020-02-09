using GTANetworkAPI;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Enum.Challenge;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Maps
{
    internal static class MapsRatings
    {
        public static async void AddPlayerMapRating(TDSPlayer player, int mapId, byte rating)
        {
            int playerId = player.Entity!.Id;

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
                maprating = new PlayerMapRatings { PlayerId = playerId, MapId = mapId };
                dbContext.PlayerMapRatings.Add(maprating);
                player.AddToChallenge(EChallengeType.ReviewMaps);
            }
            maprating.Rating = rating;
            map.BrowserSyncedData.Rating = rating;

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
            NAPI.ClientEvent.TriggerClientEvent(character.Player, DToClientEvent.LoadOwnMapRatings, Serializer.ToBrowser(ratingsDict));
        }
    }
}