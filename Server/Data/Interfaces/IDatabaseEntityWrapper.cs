using System;
using System.Threading.Tasks;
using TDS_Server.Database.Entity;

namespace TDS_Server.Data.Interfaces
{
    public interface IDatabaseEntityWrapper
    {
        #region Public Methods

        Task ExecuteForDB(Action<TDSDbContext> action);

        Task<T> ExecuteForDB<T>(Func<TDSDbContext, T> action);

        Task ExecuteForDBAsync(Func<TDSDbContext, Task> action);

        Task<T> ExecuteForDBAsync<T>(Func<TDSDbContext, Task<T>> action);

        #endregion Public Methods
    }
}
