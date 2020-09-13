﻿using System;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;

namespace TDS_Server.Handler
{
    public class DatabaseHandler : IDatabaseEntityWrapper
    {
        protected ILoggingHandler LoggingHandler;

        private readonly TDSDbContext _dbContext;

        private readonly SemaphoreSlim _dbContextSemaphore = new SemaphoreSlim(1, 1);
        private ITDSPlayer? _player;

        public DatabaseHandler(TDSDbContext dbContext, ILoggingHandler loggingHandler)
            => (_dbContext, LoggingHandler) = (dbContext, loggingHandler);

        public void SetPlayerSource(ITDSPlayer player)
            => _player = player;

        /*public void InitDbContext()
        {
            _dbContext = new TDSDbContext();
        }*/

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
    }
}
