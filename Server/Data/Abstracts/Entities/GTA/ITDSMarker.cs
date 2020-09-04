using GTANetworkAPI;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSMarker : Marker
    {
        public ITDSMarker(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
