using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Events;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Maps.MapCreatorRewards
{
    public class MapCreatorRewardsOfflineNotificationHandler
    {
        private readonly DatabaseHandler _databaseHandler;

        public MapCreatorRewardsOfflineNotificationHandler(EventsHandler eventsHandler, DatabaseHandler databaseHandler)
        {
            _databaseHandler = databaseHandler;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
        }

        public async Task Add(int playerId, IRoundFightLobby lobby, int reward)
        {
            var entry = new PlayerMapCreatorRewardsWhileOffline
            {
                MapId = lobby.CurrentMap.BrowserSyncedData.Id,
                PlayerId = playerId,
                Reward = reward,
                Type = lobby.MapHandler.MapRetrieveType
            };
            await _databaseHandler.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.PlayerMapCreatorRewardsWhileOffline.Add(entry);
                await dbContext.SaveChangesAsync();
            });
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer player)
        {
            var rewardEntries = await _databaseHandler.ExecuteForDBAsync(async dbContext =>
            {
                var entries = await dbContext.PlayerMapCreatorRewardsWhileOffline.Where(e => e.PlayerId == player.Id).Include(e => e.Map).ToListAsync();
                var groupedEntries = entries
                    .GroupBy(e => e.Map.Name)
                    .Select(e => new
                    {
                        RewardTotal = e.Sum(ee => ee.Reward),
                        AmountRandom = e.Count(ee => ee.Type == MapRetrieveType.Random),
                        AmountVoted = e.Count(ee => ee.Type == MapRetrieveType.Voted),
                        AmountBought = e.Count(ee => ee.Type == MapRetrieveType.Bought),
                        MapName = e.Key
                    });

                dbContext.PlayerMapCreatorRewardsWhileOffline.RemoveRange(entries);
                await dbContext.SaveChangesAsync();

                return groupedEntries;
            });

            if (rewardEntries?.Any() != true)
                return;

            foreach (var entry in rewardEntries)
                player.Chat.SendChatMessage(
                    string.Format(player.Language.MAP_CREATOR_REWARD_OFFLINE_INFO, entry.MapName, entry.RewardTotal, entry.AmountRandom, entry.AmountVoted, entry.AmountBought));
        }
    }
}