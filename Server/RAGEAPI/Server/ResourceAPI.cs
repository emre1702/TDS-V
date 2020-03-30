using TDS_Server.Data.Interfaces.ModAPI.Server;

namespace TDS_Server.RAGEAPI.Server
{
    class ResourceAPI : IResourceAPI
    {
        public bool StopThis()
        {
            return GTANetworkAPI.NAPI.Resource.StopResource("tds");
        }
    }
}
