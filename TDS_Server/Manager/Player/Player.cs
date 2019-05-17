using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Player
{
    internal static class Player
    {
        private static readonly Dictionary<Client, TDSPlayer> _clientPlayers = new Dictionary<Client, TDSPlayer>();

        public static TDSPlayer GetChar(this Client client)
        {
            if (!_clientPlayers.ContainsKey(client))
            {
                TDSPlayer player = new TDSPlayer(client);
                _clientPlayers[client] = player;
                return player;
            }
            return _clientPlayers[client];
        }

        public static Players? GetEntity(this Client client)
        {
            return GetChar(client).Entity;
        }
        public static async Task<bool> DoesPlayerWithScnameExist(string scname)
        {
            return await GetPlayerIDByScname(scname) != 0;
        }

        public static async Task<int> GetPlayerIDByScname(string scname)
        {
            using var dbcontext = new TDSNewContext();
            return await dbcontext.Players
                .Where(p => p.Scname == scname)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }
    }
}