using GTANetworkAPI;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GTA.Vehicles
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
