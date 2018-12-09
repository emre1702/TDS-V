using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TDS.Default;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Manager.Player;

namespace TDS.Manager.Maps
{

    static partial class Maps
    {

        private static Dictionary<string, uint[]> mapRatingDict = new Dictionary<string, uint[]>();
        private static Dictionary<uint, Dictionary<string, uint>> playerMapRating = new Dictionary<uint, Dictionary<string, uint>>();


        public static async void AddPlayerMapRating(Client player, string mapname, byte rating)
        {
            //if (!DoesMapNameExist(mapname))
                //return;

            uint playerid = player.GetEntity().Id;
            using (var dbcontext = new TDSNewContext())
            {
                Playermapratings maprating = await dbcontext.Playermapratings.FindAsync(playerid, mapname);
                if (maprating == null)
                {
                    maprating = new Playermapratings { Id = playerid, MapName = mapname };
                    await dbcontext.Playermapratings.AddAsync(maprating);
                }
                maprating.Rating = rating;
                await dbcontext.SaveChangesAsync();
            }
        }

        public static async void SendPlayerHisRatingsAsync(Character character)
        {
            using (var dbcontext = new TDSNewContext())
            {
                List<Playermapratings> list = await dbcontext.Playermapratings.Where(rating => rating.Id == character.Entity.Id).ToListAsync();
                if (list.Count > 0)
                {
                    NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.ClientLoadOwnMapRatings, JsonConvert.SerializeObject(list));
                }
            }
        }

    }
}