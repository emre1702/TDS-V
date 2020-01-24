using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.PlayerManager
{
    internal static class PlayerManager
    {
        public static int AmountLoggedInPlayers => LoggedInPlayers.Count;
        public static readonly List<TDSPlayer> LoggedInPlayers = new List<TDSPlayer>();

        private static readonly Dictionary<Player, TDSPlayer> _clientPlayers = new Dictionary<Player, TDSPlayer>();

        static PlayerManager()
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

        public static TDSPlayer GetChar(this Player client)
        {
            if (!_clientPlayers.ContainsKey(client))
            {
                TDSPlayer player = new TDSPlayer(client);
                _clientPlayers[client] = player;
                return player;
            }
            return _clientPlayers[client];
        }

        public static Players? GetEntity(this Player client)
        {
            return GetChar(client).Entity;
        }

        public static async Task<bool> DoesPlayerWithScnameExist(string scname)
        {
            return await GetPlayerIDByScname(scname).ConfigureAwait(false) != 0;
        }

        public static async Task<bool> DoesPlayerWithNameExist(string name)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players.AsNoTracking()
                            .AnyAsync(p => EF.Functions.ILike(p.Name, name)).ConfigureAwait(false);    
        }

        public static async Task<int> GetPlayerIDByScname(string scname)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players
                .AsNoTracking()
                .Where(p => p.SCName == scname)
                .Select(p => p.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public static async Task<int> GetPlayerIDByName(string name)
        {
            using var dbContext = new TDSDbContext();
            return await dbContext.Players
                .AsNoTracking()
                .Where(p => p.Name == name)
                .Select(p => p.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public static TDSPlayer? GetPlayerByID(int id)
        {
            return LoggedInPlayers.FirstOrDefault(p => p.Entity?.Id == id);
        }

        public static void SetAllLoggedOutInDb(TDSDbContext dbContext)
        {
            /*var entityInfo = dbContext.GetDbEntityType(typeof(PlayerStats));
            if (entityInfo is null)
                return;
            string tableName = dbContext.GetTableName(entityInfo);
            var propertyName = dbContext.GetPropertyName(entityInfo, nameof(PlayerStats.LoggedIn));

            dbContext.Database.ExecuteSqlRaw($"UPDATE {tableName} SET {propertyName} = false");*/

            dbContext.PlayerStats.Where(p => p.LoggedIn).UpdateFromQuery(p => new PlayerStats { LoggedIn = false });
        }
    }
}
