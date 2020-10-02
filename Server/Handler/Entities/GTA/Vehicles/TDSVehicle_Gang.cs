using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GTA.Vehicles
{
    partial class TDSVehicle
    {
        public override void SetGang(IGang gang)
        {
            NumberPlate = gang.Entity.Short;
            _dataSyncHandler.SetData(this, EntityDataKey.GangId, DataSyncMode.Lobby, gang.Entity.Id, toLobby: Lobby);
        }
    }
}
