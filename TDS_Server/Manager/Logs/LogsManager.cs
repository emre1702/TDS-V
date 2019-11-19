using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Log;

namespace TDS_Server.Manager.Logs
{
    static class LogsManager
    {
        private static readonly TDSDbContext _dbContext;

        private static readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1);
        private static bool _usingDBContext;

#pragma warning disable CA1810 // Initialize reference type static fields inline
        static LogsManager()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            _dbContext = new TDSDbContext();
        }

        public static async Task Save()
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
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

        public static async void AddLog(LogRests logs)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                _dbContext.LogRests.Add(logs);
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

        public static async void AddLog(LogErrors logs)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                _dbContext.LogErrors.Add(logs);
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

        public static async void AddLog(LogChats logs)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                _dbContext.LogChats.Add(logs);
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

        public static async void AddLog(LogAdmins logs)
        {
            bool wasInDBContextBefore = _usingDBContext;
            if (!wasInDBContextBefore)
            {
                await _dbContextSemaphore.WaitAsync();
                _usingDBContext = true;
            }

            try
            {
                _dbContext.LogAdmins.Add(logs);
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
    }
}
