using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity;

namespace TDS.Server.Data.Interfaces.Entities
{
    public interface IDatabaseHandler : IDatabaseEntityWrapper, IAsyncDisposable
    {
        void ExecuteForDBWithoutWait(Action<TDSDbContext> action);

        void SetPlayerSource(ITDSPlayer player);
    }
}
