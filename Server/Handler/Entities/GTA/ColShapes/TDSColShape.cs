using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Handler.Entities.GTA.ColShapes
{
    public partial class TDSColShape : ITDSColshape
    {
        public TDSColShape(NetHandle netHandle) : base(netHandle) => AddEvents();
    }
}
