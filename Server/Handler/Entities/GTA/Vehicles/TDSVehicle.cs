using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Handler.Sync;

namespace TDS.Server.Handler.Entities.GTA.Vehicles
{
    public partial class TDSVehicle : ITDSVehicle
    {
        private readonly IWorkaroundsHandler _workaroundsHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public TDSVehicle(NetHandle netHandle,
            IWorkaroundsHandler workaroundsHandler, DataSyncHandler dataSyncHandler) : base(netHandle)
            => (_workaroundsHandler, _dataSyncHandler) = (workaroundsHandler, dataSyncHandler);
    }
}