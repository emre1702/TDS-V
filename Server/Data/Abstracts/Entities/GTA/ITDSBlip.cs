using GTANetworkAPI;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSBlip : Blip
    {

        public ITDSBlip(NetHandle netHandle) : base(netHandle) { }
    }
}
