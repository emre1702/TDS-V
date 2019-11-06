using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Utility
{
    abstract class EntityWrapperClass
    {
        protected TDSNewContext DbContext
        {
            get
            {
                if (_dbContext is null)
                    _dbContext = new TDSNewContext();
                return _dbContext;
            }
            set => _dbContext = value;
        }

        private TDSNewContext? _dbContext;
        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1);
        private bool _usingDBContext;

        public void InitDbContext()
        {
            _dbContext = new TDSNewContext();
        }

        public async Task ExecuteForDBAsync(Func<TDSNewContext, Task> action)
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

        public async Task<T> ExecuteForDBAsync<T>(Func<TDSNewContext, Task<T>> action)
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

        public async Task ExecuteForDB(Action<TDSNewContext> action)
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
        }
    }
}
