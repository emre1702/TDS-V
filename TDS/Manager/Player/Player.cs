using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TDS.Manager.Player
{
    static class Player
    {
        private static ConditionalWeakTable<Client, Entities.Players> playerEntities = new ConditionalWeakTable<Client, Entities.Players>(); 

        /// <summary>
        /// Get the EF Entity for a player.
        /// First look for it in playerEntities (caching) - if it's not there, search for it in the Database.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Entities.Players GetEntity(this Client player)
        {
            return playerEntities.GetValue(player, (Client pl) =>
            {
                using (var dbcontext = new Entities.TDSNewContext())
                {
                    Entities.Players entity = dbcontext.Players.FirstOrDefault(p => p.Scname == pl.SocialClubName);
                    if (entity != null)
                    {
                        playerEntities.Add(pl, entity);
                    }
                    return entity;
                }
            });
        }

        public static Entities.Playersettings GetSettings(this Client player)
        {
            return GetEntity(player)?.Playersettings;
        }


    }
}
