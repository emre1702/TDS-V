using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Challenge;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Challenge;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Helper
{
    public class ChallengesHelper : DatabaseEntityWrapper
    {
        private readonly SettingsHandler _settingsHandler;
        private readonly Serializer _serializer;
        private readonly IModAPI _modAPI;

        public ChallengesHelper(
            SettingsHandler settingsHandler, 
            EventsHandler eventsHandler, 
            LoggingHandler loggingHandler, 
            TDSDbContext dbContext,
            Serializer serializer,
            IModAPI modAPI) : base(dbContext, loggingHandler)
        { 
            _settingsHandler = settingsHandler;
            _serializer = serializer;
            _modAPI = modAPI;

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerRegistered += EventsHandler_PlayerRegister;
        }

        private async void EventsHandler_PlayerLoggedIn(ITDSPlayer iplayer)
        {
            if (!(iplayer is TDSPlayer player))
                return;
            if (player.Entity is null)
                return;

            if (!player.Entity.Challenges.Any(c => c.Frequency == ChallengeFrequency.Weekly))
            {
                await AddWeeklyChallenges(player);
                await player.ExecuteForDBAsync(async dbContext =>
                {
                    player.Entity.Challenges = null;
                    dbContext.Entry(player.Entity).Collection(p => p.Challenges).IsLoaded = false;
                    await dbContext.Entry(player.Entity).Collection(p => p.Challenges).LoadAsync();
                });
            }
            player.InitChallengesDict();
            player.SendBrowserEvent(ToBrowserEvent.SyncChallenges, GetChallengesJson(player));
        }

        private async void EventsHandler_PlayerRegister(ITDSPlayer player)
        {
            try 
            {
                await AddForeverChallenges(player);
            } 
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, player);
            }
        }

        public void ClearWeeklyChallenges()
        {
            ExecuteForDB(dbContext =>
            { 
                string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));

                string sql = $"DELETE FROM {playerChallengesTable} WHERE frequency = 'weekly'";
                dbContext.Database.ExecuteSqlRaw(sql);
            }).RunSynchronously();
        }

        public async Task AddWeeklyChallenges(ITDSPlayer player)
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
                string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

                string sql = @$"
                INSERT INTO 
                    {playerChallengesTable}
                SELECT 
                    {player.Id},
                    type,
                    frequency,
                    floor(random() * (max_number - min_number+1) + min_number)
                FROM
                    {challengeSettingsTable}
                TABLESAMPLE SYSTEM_ROWS({_settingsHandler.ServerSettings.AmountWeeklyChallenges})
                WHERE 
                    frequency = 'weekly'
                ";
                await dbContext.Database.ExecuteSqlRawAsync(sql);
            });
            
        }

        public async Task AddForeverChallenges(ITDSPlayer player)
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                string playerChallengesTable = dbContext.GetTableName(typeof(PlayerChallenges));
                string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

                string sql = $@"
                    INSERT INTO 
                        {playerChallengesTable}
                    SELECT 
                        {player.Id},
                        type,
                        frequency,
                        max_number
                    FROM
                        {challengeSettingsTable}
                    WHERE 
                        frequency = 'forever'
                ";
                await dbContext.Database.ExecuteSqlRawAsync(sql);
            });
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
