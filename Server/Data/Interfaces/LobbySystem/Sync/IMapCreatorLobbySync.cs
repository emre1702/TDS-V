using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Server.Data.Interfaces.LobbySystem.Sync
{
    public interface IMapCreatorLobbySync : IBaseLobbySync
    {
        Task SetMap(MapCreateDataDto dto);

        void StartNewMap();

        void SyncLastId(ITDSPlayer player, int lastId);

        void SyncMapInfoChange(MapCreatorInfoType infoType, object data);

        void SyncNewObject(ITDSPlayer player, string json);

        void SyncObjectPosition(ITDSPlayer player, string json);

        void SyncRemoveObject(ITDSPlayer player, int objId);

        void SyncRemoveTeamObjects(ITDSPlayer player, int teamNumber);

        Task SetSyncedMapAndSyncToPlayer(string json, int tdsPlayerId, int lastId);
    }
}
