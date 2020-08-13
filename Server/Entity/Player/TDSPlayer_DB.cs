using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private Players? _entity;
        private int _lastSaveTick;
        private readonly TDSDbContext _dbContext;

        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);

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


        public async Task ExecuteForDB(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite);

            try
            {
                action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, _player);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task<T> ExecuteForDB<T>(Func<TDSDbContext, T> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite);

            try
            {
                return action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, _player);
                return default!;
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task ExecuteForDBAsync(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite);

            try
            {
                await action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, _player);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite);

            try
            {
                return await action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, _player);
                return default!;
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async void ExecuteForDBAsyncWithoutWait(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite);

            try
            {
                await action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError(ex, _player);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        #endregion Public Methods
    }
}
