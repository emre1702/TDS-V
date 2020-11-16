using GTANetworkAPI;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSBlip : Blip
    {
        protected ITDSBlip(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
