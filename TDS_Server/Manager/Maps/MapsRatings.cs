using GTANetworkAPI;
using Newtonsoft.Json;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto.Map;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Maps
{
    internal static class MapsRatings
    {
        public static async void AddPlayerMapRating(Client player, string mapName, byte rating)
        {
            uint? playerId = player.GetEntity()?.Id;
            if (playerId == null)
                return;

            MapDto? map = MapsLoader.GetMapByName(mapName) ?? MapCreator.GetMapByName(mapName);
            if (map == null)
                return;
            
            using var dbcontext = new TDSNewContext();
            PlayerMapRatings? maprating = await dbcontext.PlayerMapRatings.FindAsync(playerId, mapName);
            if (maprating == null)
            {
                maprating = new PlayerMapRatings { Id = playerId.Value, MapName = mapName };
                await dbcontext.PlayerMapRatings.AddAsync(maprating);
            }
            maprating.Rating = rating;
            await dbcontext.SaveChangesAsync();
        }

        public static void SendPlayerHisRatings(TDSPlayer character)
        {
            if (character.Entity == null)
                return;
            if (!character.Entity.PlayerMapRatings.Any())
                return;

            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.LoadOwnMapRatings, JsonConvert.SerializeObject(character.Entity.PlayerMapRatings));
        }
    }
}