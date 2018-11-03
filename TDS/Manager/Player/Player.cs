using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Entity;
using TDS.Instance.Player;
using TDS.Manager.Logs;

namespace TDS.Manager.Player
{
    static class Player
    {
        private static ConditionalWeakTable<Client, Players> clientEntities = new ConditionalWeakTable<Client, Players>();
        private static readonly ConditionalWeakTable<Client, Character> clientPlayers = new ConditionalWeakTable<Client, Character>();

        public static Client GetClient(this Players character)
        {
            foreach (KeyValuePair<Client, Players> entry in clientEntities)
            {
                if (entry.Value == character)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public static Character GetChar(this Client client)
        {
           return clientPlayers.GetValue(client, (Client c) =>
           {
                Character player = new Character
                {
                    Player = c
                };
                clientPlayers.Add(c, player);
                return player;
           });
        }

        public static Players GetEntity(this Client client)
        {
            return GetChar(client)?.Entity;
        }

        public static Client GetPlayer(uint id)
        {
            foreach (var entry in clientPlayers)
            {
                if (entry.Value.Entity.Id == id)
                {
                    return entry.Value.Player;
                }
            }
            return null;
        }

        public static Playersettings GetSettings(this Client player)
        {
            return player.GetChar().Entity.Playersettings;
        }

        public static async Task<Players> GetEntityByID(uint id)
        {
            using (var dbcontext = new TDSNewContext())
            {
                return await dbcontext.Players.FindAsync(id);
            }
        }

        public static async Task<bool> DoesPlayerWithScnameExist(string scname)
        {
            return await GetPlayerIDByScname(scname) != 0;
        }

        public static async Task<uint> GetPlayerIDByScname(string scname)
        {
            // Check our cache first
            var idobj = clientEntities
                        .Where(pl => pl.Value.Scname == scname)
                        .Select(pl => new { ID = pl.Value.Id })
                        .FirstOrDefault();
            bool exists = idobj != null;
            if (!exists)
            {
                // Now check the database
                using (var dbcontext = new TDSNewContext())
                {
                    idobj = (await dbcontext.Players
                                .Where(p => p.Scname == scname)
                                .Select(p => new { ID = p.Id })
                                .ToListAsync()
                            ).FirstOrDefault();
                    exists = idobj != null;

                }
            }
            return exists ? idobj.ID : 0;
        }



        public static void GiveMoney(this Client player, int money)
        {
            Players entity = player.GetChar().Entity;
            if (money >= 0 || entity.Playerstats.Money > money * -1)
            {
                entity.Playerstats.Money = (uint) (entity.Playerstats.Money + money);
                NAPI.ClientEvent.TriggerClientEvent(player, DCustomEvents.ClientMoneyChange, entity.Playerstats.Money);
            }
            else
                Error.Log($"Should have went to minus money! Current: {entity.Playerstats.Money} | Substracted money: {money}", 
                                Environment.StackTrace, player);
        }
    }
}
