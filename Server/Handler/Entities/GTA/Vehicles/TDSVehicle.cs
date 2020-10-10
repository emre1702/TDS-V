using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Sync;

namespace TDS_Server.Handler.Entities.GTA.Vehicles
{
    public partial class TDSVehicle : ITDSVehicle
    {
        private readonly WorkaroundsHandler _workaroundsHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public TDSVehicle(NetHandle netHandle,
            WorkaroundsHandler workaroundsHandler, DataSyncHandler dataSyncHandler) : base(netHandle)
            => (_workaroundsHandler, _dataSyncHandler) = (workaroundsHandler, dataSyncHandler);
    }
}
