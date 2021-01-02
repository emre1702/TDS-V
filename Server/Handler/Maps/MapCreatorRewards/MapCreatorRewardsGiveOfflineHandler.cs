using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Helper;

namespace TDS.Server.Handler.Maps.MapCreatorRewards
{
    internal class MapCreatorRewardsGiveOfflineHandler
    {
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly MapCreatorRewardsOfflineNotificationHandler _offlineNotificationHandler;

        internal MapCreatorRewardsGiveOfflineHandler(DatabasePlayerHelper databasePlayerHelper, MapCreatorRewardsOfflineNotificationHandler offlineNotificationHandler)
        {
            _databasePlayerHelper = databasePlayerHelper;
            _offlineNotificationHandler = offlineNotificationHandler;
        }

        internal async Task GiveReward(int playerId, IRoundFightLobby lobby, int reward)
        {
            try
            {
                await UpdateOfflinePlayerStats(playerId, reward);

                lobby.Notifications.Send(lang => string.Format(lang.MAP_CREATOR_REWARD_INFO, lobby.CurrentMap.BrowserSyncedData.CreatorName, reward));

                await UpdateOfflinePlayerTotalStats(playerId, reward);
                await _offlineNotificationHandler.Add(playerId, lobby, reward);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, source: playerId);
            }
        }

        private async Task UpdateOfflinePlayerTotalStats(int playerId, int reward)
        {
            if (!await DoPlayerTotalStatsExist(playerId))
                return;

            var playerTotalStatsMoney = await GetCurrentPlayerTotalStatsMoney(playerId) + reward;
            var playerTotalStats = new PlayerTotalStats
            {
                PlayerId = playerId,
                Money = playerTotalStatsMoney
            };
            await UpdatePlayerTotalStats(playerTotalStats);
        }

        private async Task UpdateOfflinePlayerStats(int playerId, int reward)
        {
            if (!await DoPlayerStatsExist(playerId))
                return;

            var playerStatsMoney = await GetCurrentPlayerStatsMoney(playerId) + reward;
            var playerStats = new PlayerStats
            {
                PlayerId = playerId,
                Money = playerStatsMoney
            };
            await UpdatePlayerStats(playerStats);
        }

        private Task<bool> DoPlayerStatsExist(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerStats.AsNoTracking().AnyAsync(s => s.PlayerId == playerId));

        private Task<int> GetCurrentPlayerStatsMoney(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerStats.AsNoTracking().Where(s => s.PlayerId == playerId).Select(s => s.Money).FirstOrDefaultAsync());

        private Task UpdatePlayerStats(PlayerStats playerStats)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.PlayerStats.Attach(playerStats);
                dbContext.Entry(playerStats).Property(s => s.Money).IsModified = true;

                await dbContext.SaveChangesAsync();
            }, dbContext => dbContext.Entry(playerStats).State = EntityState.Detached);

        private Task UpdatePlayerTotalStats(PlayerTotalStats playerTotalStats)
           => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
           {
               dbContext.PlayerTotalStats.Attach(playerTotalStats);
               dbContext.Entry(playerTotalStats).Property(s => s.Money).IsModified = true;

               await dbContext.SaveChangesAsync();
           }, dbContext => dbContext.Entry(playerTotalStats).State = EntityState.Detached);

        private Task<bool> DoPlayerTotalStatsExist(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerTotalStats.AsNoTracking().AnyAsync(s => s.PlayerId == playerId));

        private Task<long> GetCurrentPlayerTotalStatsMoney(int playerId)
            => _databasePlayerHelper.ExecuteForDBAsync(async dbContext =>
                   await dbContext.PlayerTotalStats.AsNoTracking().Where(s => s.PlayerId == playerId).Select(s => s.Money).FirstOrDefaultAsync());
    }
}