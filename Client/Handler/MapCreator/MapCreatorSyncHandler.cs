﻿using System;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorSyncHandler
    {
        public bool HasToSync => true;    // => Team.AmountPlayersSameTeam > 1;

        private readonly MapCreatorObjectsHandler _mapCreatorObjectsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly Serializer _serializer;
        private readonly BrowserHandler _browserHandler;

        public MapCreatorSyncHandler(IModAPI modAPI, MapCreatorObjectsHandler mapCreatorObjectsHandler, RemoteEventsSender remoteEventsSender, Serializer serializer, 
            EventsHandler eventsHandler, BrowserHandler browserHandler)
        {
            _mapCreatorObjectsHandler = mapCreatorObjectsHandler;
            _remoteEventsSender = remoteEventsSender;
            _serializer = serializer;
            _browserHandler = browserHandler;

            eventsHandler.MapCreatorSyncLatestObjectID += SyncLatestIdToServer;
            eventsHandler.MapCreatorSyncObjectDeleted += SyncObjectRemoveToLobby;

            modAPI.Event.Add(FromBrowserEvent.MapCreatorStartNew, SyncStartNewMap);
            modAPI.Event.Add(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, OnMapCreatorRequestAllObjectsForPlayerMethod);
            modAPI.Event.Add(ToClientEvent.MapCreatorSyncAllObjects, OnMapCreatorSyncAllObjectsMethod);
            modAPI.Event.Add(ToClientEvent.MapCreatorSyncFixLastId, OnMapCreatorSyncFixLastIdMethod);
            modAPI.Event.Add(ToClientEvent.MapCreatorSyncNewObject, OnMapCreatorSyncNewObjectMethod);
            modAPI.Event.Add(ToClientEvent.MapCreatorSyncObjectPosition, OnMapCreatorSyncObjectPositionMethod);
            modAPI.Event.Add(ToClientEvent.MapCreatorSyncObjectRemove, MapCreatorSyncObjectRemoveMethod);
        }

        #region Id
        public void SyncLatestIdToServer()
        {
            int lastUsedId = _mapCreatorObjectsHandler.IdCounter;
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncLastId, lastUsedId);
        }

        public void SyncLatestIdFromServer(int oldId, int newId)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(oldId);
            if (obj != null)
            {
                obj.ID = newId;
            }
            _mapCreatorObjectsHandler.IdCounter = Math.Max(_mapCreatorObjectsHandler.IdCounter, newId);
        }
        #endregion Id

        #region New object
        public void SyncNewObjectToLobby(MapCreatorObject obj)
        {
            obj.IsSynced = true;
            if (!HasToSync)
                return;
            var dto = obj.GetDto();
            var json = _serializer.ToServer(dto);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncNewObject, json);
        }

        public void SyncNewObjectFromLobby(MapCreatorPosition dto)
        {
            var obj = _mapCreatorObjectsHandler.FromDto(dto);
            obj.IsSynced = true;
        }
        #endregion New object

        #region Object position
        public void SyncObjectPositionToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            var posDto = obj.GetPosDto();
            var json = _serializer.ToServer(posDto);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncObjectPosition, json);
        }

        public void SyncObjectPositionFromLobby(MapCreatorPosData dto)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(dto.Id);
            if (obj is null)
                return;
            obj.LoadPos(dto);
        }
        #endregion Object position

        #region Remove object
        public void SyncObjectRemoveToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncRemoveObject, obj.ID);
        }

        public void SyncObjectRemoveFromLobby(int id)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(id);
            if (obj != null)
            {
                obj.Delete(false);
            }
        }
        #endregion Remove object

        #region All objects
        public void SyncAllObjectsToPlayer(int tdsPlayerId)
        {
            var objects = _mapCreatorObjectsHandler.GetAll();
            foreach (var obj in objects)
            {
                obj.IsSynced = true;
            }
            var dtoList = objects.Select(o => o.GetDto()).ToList();
            string json = _serializer.ToServer(dtoList);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncAllObjects, tdsPlayerId, json);

            // Todo: Add P2P here as alternative (if activated, else with server)
        }

        public void SyncAllObjectsFromLobbyOwner(MapCreateDataDto data)
        {
            foreach (var dto in data.GetAllPositions)
            {
                var newObj = _mapCreatorObjectsHandler.FromDto(dto);
                _mapCreatorObjectsHandler.IdCounter = Math.Max(_mapCreatorObjectsHandler.IdCounter, newObj.ID);
            }
        }
        #endregion All objects

        #region Change map 
        public void SyncStartNewMap(params object[] args)
        {
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorStartNewMap);
        }
        #endregion Change map

        private void OnMapCreatorRequestAllObjectsForPlayerMethod(object[] args)
        {
            int tdsPlayerId = Convert.ToInt32(args[0]);
            SyncAllObjectsToPlayer(tdsPlayerId);
        }

        private void OnMapCreatorSyncAllObjectsMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var data = _serializer.FromServer<MapCreateDataDto>(json);
            SyncAllObjectsFromLobbyOwner(data);
            _browserHandler.Angular.LoadMapForMapCreator(json);
        }

        private void OnMapCreatorSyncFixLastIdMethod(object[] args)
        {
            int oldId = Convert.ToInt32(args[0]);
            int newId = Convert.ToInt32(args[1]);
            SyncLatestIdFromServer(oldId, newId);
        }

        private void OnMapCreatorSyncNewObjectMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var dto = _serializer.FromServer<MapCreatorPosition>(json);
            SyncNewObjectFromLobby(dto);
        }

        private void OnMapCreatorSyncObjectPositionMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var dto = _serializer.FromServer<MapCreatorPosData>(json);
            SyncObjectPositionFromLobby(dto);
        }

        private void MapCreatorSyncObjectRemoveMethod(object[] args)
        {
            int objId = Convert.ToInt32(args[0]);
            SyncObjectRemoveFromLobby(objId);
        }
    }
}