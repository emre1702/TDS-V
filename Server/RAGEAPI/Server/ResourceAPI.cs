using TDS_Server.Data.Interfaces.ModAPI.Server;

namespace TDS_Server.RAGEAPI.Server
{
    internal class ResourceAPI : IResourceAPI
    {
        #region Public Methods

        public bool StopThis()
        {
            return GTANetworkAPI.NAPI.Resource.StopResource("tds");
        }

        #endregion Public Methods
    }
}
