using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Handler.Entities.GTA.Blips
{
    public class TDSBlip : ITDSBlip
    {
        public TDSBlip(NetHandle netHandle) : base(netHandle)
        {

        }
    }
}
