using GTANetworkAPI;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSColShape : ColShape
    {
        public ITDSColShape(NetHandle netHandle): base(netHandle) { }
    }
}
