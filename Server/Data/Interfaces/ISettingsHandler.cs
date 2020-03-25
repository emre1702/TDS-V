using TDS_Server.Database.Entity.Server;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Interfaces
{
    public interface ISettingsHandler
    {
        ServerSettings ServerSettings { get; }
        SyncedServerSettingsDto SyncedSettings { get; }

        bool CanLoadMapsFromOthers(ITDSPlayer player);
    }
}
