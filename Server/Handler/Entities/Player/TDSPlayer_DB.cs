using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private Players? _entity;
        private int _lastSaveTick;

        #endregion Private Fields

        #region Public Properties

        public Players? Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                if (_entity is null)
                    return;
                if (_langEnumBeforeLogin != TDS_Shared.Data.Enums.Language.English)
                    _entity.PlayerSettings.Language = _langEnumBeforeLogin;
                Language = _langHelper.GetLang(_entity.PlayerSettings.Language);
                LoadRelations();
                _dataSyncHandler.SetData(this, PlayerDataKey.Money, DataSyncMode.Player, _entity.PlayerStats.Money);
                _dataSyncHandler.SetData(this, PlayerDataKey.AdminLevel, DataSyncMode.All, _entity.AdminLvl);
                LoadTimezone();
                LoadWeaponStats();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public async void CheckSaveData()
        {
            if (Environment.TickCount - _lastSaveTick < _settingsHandler.ServerSettings.SavePlayerDataCooldownMinutes * 60 * 1000)
                return;

            await SaveData().ConfigureAwait(false);
        }

        public async ValueTask SaveData(bool force = false)
        {
            if (!force && (Entity is null || !Entity.PlayerStats.LoggedIn))
                return;

            _lastSaveTick = Environment.TickCount;
            await ExecuteForDBAsync(async (dbContext) =>
            {
                if (LobbyStats is { } && _lobbiesHandler.GetLobby(LobbyStats.LobbyId) is null)
                {
                    dbContext.Entry(LobbyStats).State = EntityState.Detached;
                    LobbyStats = null;
                }
                await dbContext.SaveChangesAsync();
            }).ConfigureAwait(false);
        }

        #endregion Public Methods
    }
}
