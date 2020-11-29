using System;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Database.Entity;

namespace TDS.Server.Handler
{
    public class DatabaseHandler : IDatabaseHandler
    {
        protected ILoggingHandler LoggingHandler { get; }

        private readonly TDSDbContext _dbContext;

        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);
        private ITDSPlayer? _player;

        public DatabaseHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler)
            => (_dbContext, LoggingHandler) = (dbContext, loggingHandler);

        public void SetPlayerSource(ITDSPlayer player)
            => _player = player;

        public async Task ExecuteForDB(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

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

        public async Task ExecuteForDBUnsafe(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                action(_dbContext);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task<T> ExecuteForDB<T>(Func<TDSDbContext, T> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

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

        public async Task<T> ExecuteForDBUnsafe<T>(Func<TDSDbContext, T> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                return action(_dbContext);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task ExecuteForDBAsync(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                await action(_dbContext).ConfigureAwait(false);
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

        public async Task ExecuteForDBAsyncUnsafe(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                await action(_dbContext).ConfigureAwait(false);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                return await action(_dbContext).ConfigureAwait(false);
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

        public async Task<T> ExecuteForDBAsyncUnsafe<T>(Func<TDSDbContext, Task<T>> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                return await action(_dbContext).ConfigureAwait(false);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async void ExecuteForDBWithoutWait(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

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

        public async void ExecuteForDBWithoutWaitUnsafe(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                action(_dbContext);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async void ExecuteForDBAsyncWithoutWait(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                await action(_dbContext).ConfigureAwait(false);
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

        public async void ExecuteForDBAsyncWithoutWaitUnsafe(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                await action(_dbContext).ConfigureAwait(false);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task Save()
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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

        public async ValueTask DisposeAsync()
        {
            await (_dbContext?.DisposeAsync() ?? default);
            _dbContextSemaphore?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
