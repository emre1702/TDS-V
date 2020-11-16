using System;
using System.Threading.Tasks;
using TDS.Server.Database.Entity;

namespace TDS.Server.Data.Interfaces
{
    public interface IDatabaseEntityWrapper
    {
        Task ExecuteForDB(Action<TDSDbContext> action);

        Task<T> ExecuteForDB<T>(Func<TDSDbContext, T> action);

        Task ExecuteForDBAsync(Func<TDSDbContext, Task> action);

        Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action);

        void ExecuteForDBAsyncWithoutWait(Func<TDSDbContext, Task> action);
    }
}
