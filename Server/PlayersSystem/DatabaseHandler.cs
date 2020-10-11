using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.PlayersSystem
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

        public IDatabaseEntityWrapper Database { get; }
        private readonly ISettingsHandler _settingsHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public DatabaseHandler(IDatabaseEntityWrapper database, ISettingsHandler settingsHandler)
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
            if (Environment.TickCount - _lastSavedMs < _settingsHandler.ServerSettings.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData().ConfigureAwait(false);
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
