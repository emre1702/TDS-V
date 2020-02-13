using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum.Challenge;
using TDS_Server.Default;
using TDS_Server.Instance.PlayerInstance;
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

            string sql = $"DELETE FROM {playerChallengesTable} WHERE frequency = 'weekly'";
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public static async Task AddWeeklyChallenges(Players dbPlayer)
        {
            using var dbContext = new TDSDbContext();

            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

            string sql = @$"
                INSERT INTO 
                    {playerChallengesTable}
                SELECT 
                    {dbPlayer.Id},
                    type,
                    frequency,
                    floor(random() * (max_number - min_number+1) + min_number)
                FROM
                    {challengeSettingsTable}
                TABLESAMPLE SYSTEM_ROWS({SettingsManager.ServerSettings.AmountWeeklyChallenges})
                WHERE 
                    frequency = 'weekly'
            ";
            await dbContext.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
        }

        public static async Task AddForeverChallenges(Players dbPlayer)
        {
            using var dbContext = new TDSDbContext();

            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

            string sql = $@"
                INSERT INTO 
                    {playerChallengesTable}
                SELECT 
                    {dbPlayer.Id},
                    type,
                    frequency,
                    max_number
                FROM
                    {challengeSettingsTable}
                WHERE 
                    frequency = 'forever'
            ";
            await dbContext.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
        }


        public static void SyncCurrentAmount(TDSPlayer player, PlayerChallenges challenge)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.ToBrowserEvent, 
                DToBrowserEvent.SyncChallengeCurrentAmountChange, 
                (int)challenge.Frequency, 
                (int)challenge.Challenge,
                challenge.CurrentAmount);
        }
    }
}
