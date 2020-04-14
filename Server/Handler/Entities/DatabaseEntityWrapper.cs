using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Entities
{
    public abstract class DatabaseEntityWrapper
    {
        private TDSDbContext _dbContext;

        private ITDSPlayer? _player;
        protected readonly ILoggingHandler LoggingHandler;
        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);

        protected DatabaseEntityWrapper(TDSDbContext dbContext, ILoggingHandler loggingHandler)
            => (_dbContext, LoggingHandler, _player) = (dbContext, loggingHandler, this as ITDSPlayer);

        ~DatabaseEntityWrapper()
        {
            _dbContextSemaphore?.Dispose();
        }

        /*public void InitDbContext()
        {
            _dbContext = new TDSDbContext();
        }*/

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
    }
}
