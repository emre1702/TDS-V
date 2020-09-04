using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.Markers
{
    public class TDSMarker : ITDSMarker
    {
        public TDSMarker(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
