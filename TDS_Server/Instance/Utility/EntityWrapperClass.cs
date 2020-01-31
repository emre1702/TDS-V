using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Utility
{
    public abstract class EntityWrapperClass
    {
        private TDSPlayer? _player;

        ~EntityWrapperClass()
        {
            _dbContextSemaphore?.Dispose();
            _dbContext?.Dispose();
        }

        protected TDSDbContext DbContext
        {
            get
            {
                if (_dbContext is null)
                    _dbContext = new TDSDbContext();
                return _dbContext;
            }
            set => _dbContext = value;
        }

        private TDSDbContext? _dbContext;
        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);

        protected void SetPlayer(TDSPlayer player)
        {
            _player = player;
        }

        public void InitDbContext()
        {
            _dbContext = new TDSDbContext();
        }

        public async Task ExecuteForDBAsync(Func<TDSDbContext, Task> action)
        {
            await _dbContextSemaphore.WaitAsync(2000);

            try
            {
                await action(DbContext);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, _player);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action)
        {
            await _dbContextSemaphore.WaitAsync(2000);

            try
            {
                return await action(DbContext);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, _player);
                return default!;
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task ExecuteForDB(Action<TDSDbContext> action)
        {
            await _dbContextSemaphore.WaitAsync(2000);

            try
            {
                action(DbContext);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, _player);
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public async Task RemoveDBContext()
        {
            await ExecuteForDB((dbContext) =>
            {
                dbContext.Dispose();
                _dbContext = null;
            });
            _dbContextSemaphore?.Dispose();
        }
    }
}
