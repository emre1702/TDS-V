using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDS_Server.Default;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;

namespace TDS_Server.Manager.Player
{
    static class Player
    {
        private static readonly Dictionary<Client, TDSPlayer> clientPlayers = new Dictionary<Client, TDSPlayer>();

        public static TDSPlayer GetChar(this Client client)
        {
            if (!clientPlayers.ContainsKey(client))
            {
                TDSPlayer player = new TDSPlayer(client);
                clientPlayers[client] = player;
                return player;
            }
            return clientPlayers[client];
        }

        public static Players? GetEntity(this Client client)
        {
            return GetChar(client).Entity;
        }

        public static TDSPlayer? GetPlayer(uint id)
        {
            foreach (var entry in clientPlayers)
            {
                if (entry.Value.Entity == null)
                    continue;
                if ((entry.Value.Entity?.Id ?? 0) == id)
                    return entry.Value;
            }
            return null;
        }

        public static PlayerSettings? GetSettings(this Client player)
        {
            return player.GetChar().Entity?.PlayerSettings;
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
            using (var dbcontext = new TDSNewContext())
            {
                return await dbcontext.Players
                            .Where(p => p.Scname == scname)
                            .Select(p => p.Id)
                            .FirstOrDefaultAsync();
            }
        }

        public static void GiveMoney(this Client player, int money)
        {
            player.GetChar().GiveMoney(money);
        }
    }
}
