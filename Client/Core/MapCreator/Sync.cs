using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Dto.Map.Creator;
using TDS_Shared.Enum;
using TDS_Shared.Core;

namespace TDS_Client.Manager.MapCreator
{
    static class Sync
    {
        public static bool HasToSync => true;    // => Team.AmountPlayersSameTeam > 1;

        #region Id
        public static void SyncLatestIdToServer()
        {
            int lastUsedId = MapCreatorObject.IdCounter;
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncLastId, lastUsedId);
        }

        public static void SyncLatestIdFromServer(int oldId, int newId)
        {
            var obj = ObjectsManager.GetByID(oldId);
            if (obj != null)
            {
                obj.ID = newId;
            }
            MapCreatorObject.IdCounter = Math.Max(MapCreatorObject.IdCounter, newId);
        }
        #endregion Id

        #region New object
        public static void SyncNewObjectToLobby(MapCreatorObject obj)
        {
            obj.IsSynced = true;
            if (!HasToSync)
                return;
            var dto = obj.GetDto();
            var json = Serializer.ToServer(dto);
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncNewObject, json);
        }

        public static void SyncNewObjectFromLobby(MapCreatorPosition dto)
        {
            var obj = MapCreatorObject.FromDto(dto);
            obj.IsSynced = true;
        }
        #endregion New object

        #region Object position
        public static void SyncObjectPositionToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            var posDto = obj.GetPosDto(); 
            var json = Serializer.ToServer(posDto);
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncObjectPosition, json);
        }

        public static void SyncObjectPositionFromLobby(MapCreatorPosData dto)
        {
            var obj = ObjectsManager.GetByID(dto.Id);
            if (obj is null)
                return;
            obj.LoadPos(dto);
        }
        #endregion Object position

        #region Remove object
        public static void SyncObjectRemoveToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncRemoveObject, obj.ID);
        }

        public static void SyncObjectRemoveFromLobby(int id)
        {
            var obj = ObjectsManager.GetByID(id);
            if (obj != null)
            {
                obj.Delete(false);
            }
        }
        #endregion Remove object

        #region All objects
        public static void SyncAllObjectsToPlayer(int tdsPlayerId)
        {
            var objects = ObjectsManager.GetAll();
            foreach (var obj in objects)
            {
                obj.IsSynced = true;
            }
            var dtoList = objects.Select(o => o.GetDto()).ToList();
            string json = Serializer.ToServer(dtoList);
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncAllObjects, tdsPlayerId, json);

            // Todo: Add P2P here as alternative (if activated, else with server)
        }

        public static void SyncAllObjectsFromLobbyOwner(MapCreateDataDto data)
        {
            foreach (var dto in data.GetAllPositions)
            {
                var newObj = MapCreatorObject.FromDto(dto);
                MapCreatorObject.IdCounter = Math.Max(MapCreatorObject.IdCounter, newObj.ID);
            }
        }
        #endregion All objects

        #region Change map 
        public static void SyncStartNewMap()
        {
            EventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorStartNewMap);
        }
        #endregion Change map
    }
}
