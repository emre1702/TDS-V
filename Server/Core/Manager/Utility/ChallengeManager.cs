using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Common.Manager.Utility;
using TDS_Server.Default;
using TDS_Server.Dto.Challlenge;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Challenge;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Core.Manager.Utility
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
                ToBrowserEvent.SyncChallengeCurrentAmountChange, 
                (int)challenge.Frequency, 
                (int)challenge.Challenge,
                challenge.CurrentAmount);
        }

        public static string GetChallengesJson(TDSPlayer player)
        {
            var result = player.Entity!.Challenges
                .GroupBy(c => c.Frequency)
                .Select(g => new ChallengeGroupDto
                {
                    Frequency = g.Key,
                    Challenges = g.Select(c => new ChallengeDto
                    {
                        Type = c.Challenge,
                        Amount = c.Amount,
                        CurrentAmount = c.CurrentAmount
                    })
                });

            return Serializer.ToBrowser(result);
        }
    }
}
