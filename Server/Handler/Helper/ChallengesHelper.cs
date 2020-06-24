using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Challenge;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Challenge;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Extensions;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Helper
{
    public class ChallengesHelper : DatabaseEntityWrapper
    {
        #region Private Fields

        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private string _challengeSettingsFrequencyColumnName = string.Empty;
        private string _challengeSettingsMaxNumberColumnName = string.Empty;
        private string _challengeSettingsMinNumberColumnName = string.Empty;
        private string _challengeSettingsTableName = string.Empty;
        private string _challengeSettingsTypeColumnName = string.Empty;
        private string _playerChallengesFrequencyColumnName = string.Empty;
        private string _playerChallengesTableName = string.Empty;

        #endregion Private Fields

        #region Public Constructors

        public ChallengesHelper(
            ISettingsHandler settingsHandler,
            EventsHandler eventsHandler,
            ILoggingHandler loggingHandler,
            TDSDbContext dbContext,
            Serializer serializer,
            IModAPI modAPI) : base(dbContext, loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _serializer = serializer;
            _modAPI = modAPI;

            LoadPlayerChallengesTableData(dbContext);
            LoadChallengeSettingsTableData(dbContext);

            eventsHandler.PlayerLoggedIn += EventsHandler_PlayerLoggedIn;
            eventsHandler.PlayerRegisteredBefore += EventsHandler_PlayerRegister;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task AddForeverChallenges(Players dbPlayer)
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                string challengeSettingsTable = dbContext.GetTableName(typeof(ChallengeSettings));

                string sql = $@"
                    INSERT INTO
                        ""{_playerChallengesTableName}""
                    SELECT
                        {dbPlayer.Id},
                        ""{_challengeSettingsTypeColumnName}"",
                        ""{_challengeSettingsFrequencyColumnName}"",
                        ""{_challengeSettingsMaxNumberColumnName}""
                    FROM
                        ""{challengeSettingsTable}""
                    WHERE
                        ""{_challengeSettingsFrequencyColumnName}"" = 'forever'
                ";
                await dbContext.Database.ExecuteSqlRawAsync(sql);
            });
        }

        public async Task AddWeeklyChallenges(ITDSPlayer player)
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                string sql = @$"
                INSERT INTO
                    ""{_playerChallengesTableName}""
                SELECT
                    {player.Id},
                    ""{_challengeSettingsTypeColumnName}"",
                    ""{_challengeSettingsFrequencyColumnName}"",
                    floor(random() * (""{_challengeSettingsMaxNumberColumnName}"" - ""{_challengeSettingsMinNumberColumnName}"" + 1) + ""{_challengeSettingsMinNumberColumnName}"")
                FROM
                    ""{_challengeSettingsTableName}""
                TABLESAMPLE SYSTEM_ROWS({_settingsHandler.ServerSettings.AmountWeeklyChallenges})
                WHERE
                    ""{_challengeSettingsFrequencyColumnName}"" = 'weekly'
                ";
                await dbContext.Database.ExecuteSqlRawAsync(sql);
            });
        }

        public void ClearWeeklyChallenges()
        {
            ExecuteForDB(dbContext =>
            {
                string sql = $"DELETE FROM \"{_playerChallengesTableName}\" WHERE {_playerChallengesFrequencyColumnName} = 'weekly'";
                dbContext.Database.ExecuteSqlRaw(sql);
            }).Wait();
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

        public void SyncCurrentAmount(ITDSPlayer player, PlayerChallenges challenge)
        {
            _modAPI.Sync.SendEvent(player, ToClientEvent.ToBrowserEvent,
                ToBrowserEvent.SyncChallengeCurrentAmountChange,
                (int)challenge.Frequency,
                (int)challenge.Challenge,
                challenge.CurrentAmount);
        }

        #endregion Public Methods

        #region Private Methods

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
                    await dbContext.Entry(player.Entity).Collection(p => p.Challenges).Reload();
                });
            }
            _modAPI.Thread.QueueIntoMainThread(() =>
            {
                player.InitChallengesDict();
                player.SendBrowserEvent(ToBrowserEvent.SyncChallenges, GetChallengesJson(player));
            });
        }

        private async ValueTask EventsHandler_PlayerRegister((ITDSPlayer player, Players dbPlayer) args)
        {
            try
            {
                await AddForeverChallenges(args.dbPlayer);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, args.player);
            }
        }

        private void LoadChallengeSettingsTableData(TDSDbContext dbContext)
        {
            var challengeSettingsType = dbContext.Model.FindEntityType(typeof(ChallengeSettings));

            _challengeSettingsTableName = challengeSettingsType.GetTableName();
            var challengeSettingsEntity = new ChallengeSettings();

            foreach (var property in challengeSettingsType.GetProperties())
            {
                switch (property.Name)
                {
                    case nameof(challengeSettingsEntity.Frequency):
                        _challengeSettingsFrequencyColumnName = property.GetColumnName();
                        break;

                    case nameof(challengeSettingsEntity.MinNumber):
                        _challengeSettingsMinNumberColumnName = property.GetColumnName();
                        break;

                    case nameof(challengeSettingsEntity.MaxNumber):
                        _challengeSettingsMaxNumberColumnName = property.GetColumnName();
                        break;

                    case nameof(challengeSettingsEntity.Type):
                        _challengeSettingsTypeColumnName = property.GetColumnName();
                        break;
                }
            }
        }

        private void LoadPlayerChallengesTableData(TDSDbContext dbContext)
        {
            var playerChallengesType = dbContext.Model.FindEntityType(typeof(PlayerChallenges));

            _playerChallengesTableName = playerChallengesType.GetTableName();
            var playerChallengesEntity = new PlayerChallenges();

            _playerChallengesFrequencyColumnName = playerChallengesType.FindProperty(nameof(playerChallengesEntity.Frequency)).GetColumnName();
        }

        #endregion Private Methods
    }
}
