using TDS_Server.Data.Interfaces.ModAPI.Server;

namespace TDS_Server.RAGEAPI.Server
{
    internal class ServerAPI : IServerAPI
    {
        #region Public Methods

        public string GetName()
        {
            return GTANetworkAPI.NAPI.Server.GetServerName();
        }

        public int GetPort()
        {
            return GTANetworkAPI.NAPI.Server.GetServerPort();
        }

        #endregion Public Methods
    }
}
