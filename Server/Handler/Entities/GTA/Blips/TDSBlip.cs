using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.Blips
{
    public class TDSBlip : ITDSBlip
    {
        public TDSBlip(NetHandle netHandle) : base(netHandle)
        {

        }
    }
}
