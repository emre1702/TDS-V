using System;
using System.Threading.Tasks;
using TDS.Server.Database.Entity;

namespace TDS.Server.Data.Interfaces
{
    public interface IDatabaseEntityWrapper
    {
        Task ExecuteForDB(Action<TDSDbContext> action);
        Task ExecuteForDBUnsafe(Action<TDSDbContext> action);

        Task<T> ExecuteForDB<T>(Func<TDSDbContext, T> action);
        Task<T> ExecuteForDBUnsafe<T>(Func<TDSDbContext, T> action);

        Task ExecuteForDBAsync(Func<TDSDbContext, Task> action);
        Task ExecuteForDBAsyncUnsafe(Func<TDSDbContext, Task> action);

        Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action);
        Task<T> ExecuteForDBAsyncUnsafe<T>(Func<TDSDbContext, Task<T>> action);

        void ExecuteForDBAsyncWithoutWait(Func<TDSDbContext, Task> action);
        void ExecuteForDBAsyncWithoutWaitUnsafe(Func<TDSDbContext, Task> action);

        Task Save();
    }
}
