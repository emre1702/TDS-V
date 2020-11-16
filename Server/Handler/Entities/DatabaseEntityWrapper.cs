using System;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity;

namespace TDS.Server.Handler.Entities
{
    public abstract class DatabaseEntityWrapper : IDatabaseEntityWrapper, IAsyncDisposable
    {

        private readonly TDSDbContext _dbContext;

        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);

        protected DatabaseEntityWrapper(TDSDbContext dbContext)
            => (_dbContext) = (dbContext);

        /*public void InitDbContext()
        {
            _dbContext = new TDSDbContext();
        }*/

        public async Task ExecuteForDB(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(Timeout.Infinite).ConfigureAwait(false);

            try
            {
                action(_dbContext);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
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
                LoggingHandler.Instance.LogError(ex);
                return default!;
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
                LoggingHandler.Instance.LogError(ex);
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
                LoggingHandler.Instance.LogError(ex);
                return default!;
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
                LoggingHandler.Instance.LogError(ex);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }


        public async ValueTask DisposeAsync()
        {
            await (_dbContext?.DisposeAsync() ?? default);
            _dbContextSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
