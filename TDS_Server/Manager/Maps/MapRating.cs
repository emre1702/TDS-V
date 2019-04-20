using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TDS_Server.Default;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;
using TDS_Common.Default;

namespace TDS_Server.Manager.Maps
{

    static partial class MapsManager
    {

        private static Dictionary<string, uint[]> mapRatingDict = new Dictionary<string, uint[]>();
        private static Dictionary<uint, Dictionary<string, uint>> playerMapRating = new Dictionary<uint, Dictionary<string, uint>>();


        public static async void AddPlayerMapRating(Client player, string mapname, byte rating)
        {
            //if (!DoesMapNameExist(mapname))
                //return;

            uint? playerid = player.GetEntity()?.Id;
            if (playerid == null)
                return;
            using (var dbcontext = new TDSNewContext())
            {
                PlayerMapRatings maprating = await dbcontext.PlayerMapRatings.FindAsync(playerid, mapname);
                if (maprating == null)
                {
                    maprating = new PlayerMapRatings { Id = playerid.Value, MapName = mapname };
                    await dbcontext.PlayerMapRatings.AddAsync(maprating);
                }
                maprating.Rating = rating;
                await dbcontext.SaveChangesAsync();
            }
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