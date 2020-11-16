using GTANetworkAPI;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSMarker : Marker
    {
        protected ITDSMarker(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
