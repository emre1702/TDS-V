using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.ColShapes
{
    public partial class TDSColShape : ITDSColshape
    {
        public TDSColShape(NetHandle netHandle) : base(netHandle) => AddEvents();
    }
}
