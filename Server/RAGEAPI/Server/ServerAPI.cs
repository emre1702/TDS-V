using TDS_Server.Data.Interfaces.ModAPI.Server;

namespace TDS_Server.RAGEAPI.Server
{
    class ServerAPI : IServerAPI
    {
        public string GetName()
        {
            return GTANetworkAPI.NAPI.Server.GetServerName();
        }

        public int GetPort()
        {
            return GTANetworkAPI.NAPI.Server.GetServerPort();
        }
    }
}
