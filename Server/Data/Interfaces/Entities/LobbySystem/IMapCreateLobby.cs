using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities.LobbySystem
{
    public interface IMapCreateLobby : ILobby
    {
        void StartNewMap();
        void SyncLastId(ITDSPlayer player, int id);
        void SyncNewObject(ITDSPlayer player, string json);
        void SyncRemoveObject(ITDSPlayer player, int id);
        void SyncObjectPosition(ITDSPlayer player, string json);
        void SyncRemoveTeamObjects(ITDSPlayer player, int teamNumber);
        void GiveVehicle(ITDSPlayer player, FreeroamVehicleType vehType);
        void SyncMapInfoChange(MapCreatorInfoType infoType, object data);
    }
}
