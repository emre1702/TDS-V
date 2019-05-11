using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Maps
{
    internal static class MapsRatings
    {
        public static async void AddPlayerMapRating(Client player, string mapName, byte rating)
        {
            int? playerId = player.GetEntity()?.Id;
            if (playerId == null)
                return;

            MapDto? map = MapsLoader.GetMapByName(mapName) ?? MapCreator.GetMapByName(mapName);
            if (map == null)
                return;

            using var dbcontext = new TDSNewContext();
            var dbMap = await dbcontext.Maps.FirstOrDefaultAsync(m => m.Name == mapName);

            PlayerMapRatings? maprating = await dbcontext.PlayerMapRatings.FindAsync(playerId, dbMap.Id);
            if (maprating == null)
            {
                maprating = new PlayerMapRatings { PlayerId = playerId.Value, MapId = dbMap.Id };
                await dbcontext.PlayerMapRatings.AddAsync(maprating);
            }
            maprating.Rating = rating;
            map.SyncedData.Rating = rating;
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