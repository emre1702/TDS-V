using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum.Challenge;
using TDS_Server.Default;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Challenge;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Utility
{
    class ChallengeManager
    {
        public static void ClearWeeklyChallenges(TDSDbContext dbContext)
        {
            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));

            string sql = "DELETE FROM {0} WHERE frequency = 'weekly'";
            dbContext.Database.ExecuteSqlRaw(sql, playerChallengesTable);
        }

        public static async Task AddWeeklyChallenges(Players dbPlayer)
        {
            using var dbContext = new TDSDbContext();

            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

            string sql = @"
                INSERT INTO 
                    {0}
                SELECT 
                    {1},
                    type,
                    frequency,
                    floor(random() * (max_value - min_value+1) + min_value)
                FROM
                    {2}
                TABLESAMPLE SYSTEM_ROWS({3})
                WHERE 
                    frequency = 'weekly'
            ";
            await dbContext.Database.ExecuteSqlRawAsync(sql,
                    playerChallengesTable,
                    dbPlayer.Id,
                    challengeSettingsTable,
                    SettingsManager.ServerSettings.AmountWeeklyChallenges)
                .ConfigureAwait(false);
        }

        public static async Task AddForeverChallenges(Players dbPlayer)
        {
            using var dbContext = new TDSDbContext();

            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

            string sql = @"
                INSERT INTO 
                    {0}
                SELECT 
                    {1},
                    type,
                    frequency,
                    max_number
                FROM
                    {2}
                WHERE 
                    frequency = 'forever'
            ";
            await dbContext.Database.ExecuteSqlRawAsync(sql,
                    playerChallengesTable,
                    dbPlayer.Id, 
                    challengeSettingsTable)
                .ConfigureAwait(false);
        }


        public static void SyncCurrentAmount(TDSPlayer player, PlayerChallenges challenge)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ToBrowserEvent, 
                DToBrowserEvent.SyncChallengeCurrentAmountChange, 
                (int)challenge.Frequency, 
                (int)challenge.Challenge,
                challenge.CurrentAmount);
        }
    }
}
