using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Handler.Entities.GTA.Markers
{
    public class TDSMarker : ITDSMarker
    {
        public TDSMarker(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
