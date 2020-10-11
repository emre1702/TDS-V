using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;

namespace TDS_Server.Data.Interfaces.Entities
{
    public interface IDatabaseHandler : IDatabaseEntityWrapper
    {
        void ExecuteForDBWithoutWait(Action<TDSDbContext> action);

        void SetPlayerSource(ITDSPlayer player);
    }
}
