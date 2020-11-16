using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler;

namespace TDS.Server.PlayersSystem
{
    public class DatabaseHandler : IPlayerDatabaseHandler
    {
        public Players? Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                _player.Events.TriggerEntityChanged(value);
            }
        }

        private Players? _entity;
        private int _lastSavedMs;

        public IDatabaseHandler Database { get; }
        private readonly ISettingsHandler _settingsHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public DatabaseHandler(IDatabaseHandler database, ISettingsHandler settingsHandler)
        {
            Database = database;
            _settingsHandler = settingsHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public async void CheckSaveData()
        {
            try
            {
                if (Environment.TickCount - _lastSavedMs < _settingsHandler.ServerSettings.SavePlayerDataCooldownMinutes * 60 * 1000)
                    return;

                await SaveData().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async ValueTask SaveData(bool force = false)
        {
            if (!force && (Entity is null || !Entity.PlayerStats.LoggedIn))
                return;

            _lastSavedMs = Environment.TickCount;
            await Database.ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync().ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
