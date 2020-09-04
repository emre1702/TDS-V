using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Database.Entity.Server;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Interfaces
{
    public interface ISettingsHandler
    {
        #region Public Properties

        ServerSettings ServerSettings { get; }
        SyncedServerSettingsDto SyncedSettings { get; }

        #endregion Public Properties

        #region Public Methods

        bool CanLoadMapsFromOthers(ITDSPlayer player);

        #endregion Public Methods
    }
}
