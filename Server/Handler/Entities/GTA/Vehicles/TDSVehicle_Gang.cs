using GTANetworkAPI;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Entities.GTA.Vehicles
{
    partial class TDSVehicle
    {
        public override void SetGang(IGang gang)
        {
            NAPI.Task.RunSafe(() =>
            { 
                NumberPlate = gang.Entity.Short;
                _dataSyncHandler.SetData(this, EntityDataKey.GangId, DataSyncMode.Lobby, gang.Entity.Id, toLobby: Lobby);
            });
        }
    }
}
