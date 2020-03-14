using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Default;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Challenge;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Challenge;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Helper
{
    public class ChallengesHelper
    {
        private readonly SettingsHandler _settingsHandler;
        private readonly LoggingHandler _loggingHandler;
        private readonly TDSDbContext _dbContext;
        private readonly Serializer _serializer;
        private readonly IModAPI _modAPI;

        public ChallengesHelper(
            SettingsHandler settingsHandler, 
            EventsHandler eventsHandler, 
            LoggingHandler loggingHandler, 
            TDSDbContext dbContext,
            Serializer serializer,
            IModAPI modAPI)
        { 
            _settingsHandler = settingsHandler;
            _loggingHandler = loggingHandler;
            _dbContext = dbContext;
            _serializer = serializer;
            _modAPI = modAPI;

            eventsHandler.PlayerRegistered += EventsHandler_PlayerRegister;
        }

        private async void EventsHandler_PlayerRegister(ITDSPlayer player)
        {
            try 
            {
                await AddForeverChallenges(player.Entity);
            } 
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex, player);
            }
        }

        public void ClearWeeklyChallenges(TDSDbContext dbContext)
        {
            string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));

            string sql = $"DELETE FROM {playerChallengesTable} WHERE frequency = 'weekly'";
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public async Task AddWeeklyChallenges(Players dbPlayer)
        {
            string playerChallengesTable = _dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = _dbContext.GetTableName(typeof(ChallengeSettings));

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
                TABLESAMPLE SYSTEM_ROWS({_settingsHandler.ServerSettings.AmountWeeklyChallenges})
                WHERE 
                    frequency = 'weekly'
            ";
            await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task AddForeverChallenges(Players dbPlayer)
        {
            string playerChallengesTable = _dbContext.GetTableName(typeof(PlayerChallenges));
            string challengeSettingsTable = _dbContext.GetTableName(typeof(ChallengeSettings));

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
            await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }


        public void SyncCurrentAmount(ITDSPlayer player, PlayerChallenges challenge)
        {
            _modAPI.Sync.SendEvent(player, ToClientEvent.ToBrowserEvent,
                ToBrowserEvent.SyncChallengeCurrentAmountChange,
                (int)challenge.Frequency,
                (int)challenge.Challenge,
                challenge.CurrentAmount);
        }

        public string GetChallengesJson(ITDSPlayer player)
        {
            var result = player.Entity!.Challenges
                .GroupBy(c => c.Frequency)
                .Select(g => new ChallengeGroupModel
                {
                    Frequency = g.Key,
                    Challenges = g.Select(c => new ChallengeModel
                    {
                        Type = c.Challenge,
                        Amount = c.Amount,
                        CurrentAmount = c.CurrentAmount
                    })
                });

            return _serializer.ToBrowser(result);
        }
    }
}
