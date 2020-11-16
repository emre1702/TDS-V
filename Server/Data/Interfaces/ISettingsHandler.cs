using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity.Server;
using TDS.Shared.Data.Models;

namespace TDS.Server.Data.Interfaces
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
