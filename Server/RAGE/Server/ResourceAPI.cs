using TDS_Server.Data.Interfaces.ModAPI.Server;

namespace TDS_Server.RAGE.Server
{
    class ResourceAPI : IResourceAPI
    {
        public bool StopThis()
        {
            return GTANetworkAPI.NAPI.Resource.StopResource("tds");
        }
    }
}
