using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.EventManager;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Player
{
    internal static class Player
    {
        public static int AmountLoggedInPlayers => LoggedInPlayers.Count;
        public static readonly List<TDSPlayer> LoggedInPlayers = new List<TDSPlayer>();

        private static readonly Dictionary<Client, TDSPlayer> _clientPlayers = new Dictionary<Client, TDSPlayer>();

        static Player()
        {
            CustomEventManager.OnPlayerLoggedInBefore += player =>
            {
                LoggedInPlayers.Add(player);
            };
            CustomEventManager.OnPlayerLoggedOutBefore += player =>
            {
                LoggedInPlayers.Remove(player);
            };
        }

        public static List<TDSPlayer> GetAllTDSPlayer()
        {
            return _clientPlayers.Values.Where(p => p.LoggedIn).ToList();
        }

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

        public static async Task<bool> DoesPlayerWithNameExist(string name)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players.AsNoTracking()
                            .AnyAsync(p => EF.Functions.ILike(p.Name, name));    
        }

        public static async Task<int> GetPlayerIDByScname(string scname)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players
                .AsNoTracking()
                .Where(p => p.SCName == scname)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public static async Task<int> GetPlayerIDByName(string name)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players
                .AsNoTracking()
                .Where(p => p.Name == name)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public static TDSPlayer? GetPlayerByID(int id)
        {
            return LoggedInPlayers.FirstOrDefault(p => p.Entity?.Id == id);
        }
    }
}