using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Utility
{
    public abstract class EntityWrapperClass
    {

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
        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1);
        private bool _usingDBContext;

        public void InitDbContext()
        {
            _dbContext = new TDSDbContext();
        }

        public async Task ExecuteForDBAsync(Func<TDSDbContext, Task> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                await action(DbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
            }
        }

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                return await action(DbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
            }
        }

        public async Task ExecuteForDB(Action<TDSDbContext> action)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                action(DbContext);
            }
            finally
            {
                if (!wasInDBContextBefore)
                {
                    _dbContextSemaphore.Release();
                    _usingDBContext = false;
                }
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
