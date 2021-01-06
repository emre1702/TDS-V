using System;
using System.Collections.Generic;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Lobby;
using TDS.Client.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.Map.Creator;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorSyncHandler
    {
        private readonly BrowserHandler _browserHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly MapCreatorObjectsHandler _mapCreatorObjectsHandler;

        private readonly RemoteEventsSender _remoteEventsSender;

        public MapCreatorSyncHandler(MapCreatorObjectsHandler mapCreatorObjectsHandler, RemoteEventsSender remoteEventsSender,
            EventsHandler eventsHandler, BrowserHandler browserHandler, LobbyHandler lobbyHandler, DataSyncHandler dataSyncHandler)
        {
            _mapCreatorObjectsHandler = mapCreatorObjectsHandler;
            _remoteEventsSender = remoteEventsSender;

            _browserHandler = browserHandler;
            _lobbyHandler = lobbyHandler;
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.MapCreatorSyncLatestObjectID += SyncLatestIdToServer;
            eventsHandler.MapCreatorSyncObjectDeleted += SyncObjectRemoveToLobby;
            eventsHandler.MapCreatorSyncTeamObjectsDeleted += SyncTeamObjectsRemoveToLobby;

            Add(FromBrowserEvent.MapCreatorStartNew, SyncStartNewMap);
            Add(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, OnMapCreatorRequestAllObjectsForPlayerMethod);
            Add(ToClientEvent.MapCreatorSyncAllObjects, OnMapCreatorSyncAllObjectsMethod);
            Add(ToClientEvent.MapCreatorSyncFixLastId, OnMapCreatorSyncFixLastIdMethod);
            Add(ToClientEvent.MapCreatorSyncNewObject, OnMapCreatorSyncNewObjectMethod);
            Add(ToClientEvent.MapCreatorSyncObjectPosition, OnMapCreatorSyncObjectPositionMethod);
            Add(ToClientEvent.MapCreatorSyncObjectRemove, MapCreatorSyncObjectRemoveMethod);
            Add(ToClientEvent.MapCreatorSyncTeamObjectsRemove, MapCreatorSyncTeamObjectsRemoveMethod);
        }

        public bool HasToSync => true;

        // => Team.AmountPlayersSameTeam > 1;
        public void Start()
        {
            Tick += SyncOtherPlayers;
        }

        public void Stop()
        {
            Tick -= SyncOtherPlayers;
        }

        public void SyncAllObjectsToPlayer(int tdsPlayerId)
        {
            var objects = _mapCreatorObjectsHandler.GetAll();
            foreach (var obj in objects)
            {
                obj.IsSynced = true;
            }

            _browserHandler.Angular.MapCreatorSyncCurrentMapToServer(tdsPlayerId, _mapCreatorObjectsHandler.IdCounter);

            // Todo: Add P2P here as alternative (if activated, else with server)
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

        public void SyncLatestIdToServer()
        {
            int lastUsedId = _mapCreatorObjectsHandler.IdCounter;
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncLastId, lastUsedId);
        }

        public void SyncNewObjectFromLobby(MapCreatorPosition dto)
        {
            var obj = _mapCreatorObjectsHandler.FromDto(dto);
            obj.IsSynced = true;

            if (obj.Type == MapCreatorPositionType.MapLimit)
            {
                _mapCreatorObjectsHandler.RefreshMapLimitDisplay();
            }

            _browserHandler.Angular.AddPositionToMapCreatorBrowser(obj.ID, obj.Type, obj.Position.X, obj.Position.Y, obj.Position.Z,
                obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, dto.Info, obj.OwnerRemoteId);
        }

        public void SyncNewObjectToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            obj.IsSynced = true;
            var dto = obj.GetDto();
            var json = Serializer.ToServer(dto);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncNewObject, json);
        }

        public void SyncObjectPositionFromLobby(MapCreatorPosData dto)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(dto.Id);
            if (obj is null)
                return;
            obj.LoadPos(dto);
        }

        public void SyncObjectPositionToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            var posDto = obj.GetPosDto();
            var json = Serializer.ToServer(posDto);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncObjectPosition, json);
        }

        public void SyncObjectRemoveFromLobby(int id)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(id);
            if (obj is null)
                return;

            _browserHandler.Angular.RemovePositionInMapCreatorBrowser(obj.ID, obj.Type);
            _mapCreatorObjectsHandler.Delete(obj, false);
        }

        public void SyncObjectRemoveToLobby(MapCreatorObject obj)
        {
            if (!HasToSync)
                return;
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncRemoveObject, obj.ID);
        }

        public void SyncStartNewMap(params object[] args)
        {
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorStartNewMap);
            _mapCreatorObjectsHandler.Stop();
            _mapCreatorObjectsHandler.Start();
        }

        public void SyncTeamObjectsRemoveFromLobby(int teamNumber)
        {
            _mapCreatorObjectsHandler.DeleteTeamObjects(teamNumber, false);
            _browserHandler.Angular.RemoveTeamPositionInMapCreatorBrowser(teamNumber);
        }

        public void SyncTeamObjectsRemoveToLobby(int teamNumber)
        {
            if (!HasToSync)
                return;
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncRemoveTeamObjects, teamNumber);
        }

        private void MapCreatorSyncObjectRemoveMethod(object[] args)
        {
            int objId = Convert.ToInt32(args[0]);
            SyncObjectRemoveFromLobby(objId);
        }

        private void MapCreatorSyncTeamObjectsRemoveMethod(object[] args)
        {
            int teamNumber = Convert.ToInt32(args[0]);
            SyncTeamObjectsRemoveFromLobby(teamNumber);
        }

        private void OnMapCreatorRequestAllObjectsForPlayerMethod(object[] args)
        {
            int tdsPlayerId = Convert.ToInt32(args[0]);
            SyncAllObjectsToPlayer(tdsPlayerId);
        }

        private void OnMapCreatorSyncAllObjectsMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var data = Serializer.FromServer<MapCreateDataDto>(json);
            _mapCreatorObjectsHandler.LoadMap(data, (int)args[1]);
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
            var dto = Serializer.FromServer<MapCreatorPosition>(json);
            SyncNewObjectFromLobby(dto);
        }

        private void OnMapCreatorSyncObjectPositionMethod(object[] args)
        {
            string json = Convert.ToString(args[0]);
            var dto = Serializer.FromServer<MapCreatorPosData>(json);
            SyncObjectPositionFromLobby(dto);
        }

        private void SyncOtherPlayers(List<TickNametagData> _)
        {
            foreach (var player in _lobbyHandler.Teams.SameTeamPlayers)
            {
                if (player == RAGE.Elements.Player.LocalPlayer)
                    continue;
                if (_dataSyncHandler.GetData(PlayerDataKey.InFreeCam, false))
                {
                    player.FreezePosition(true);
                    player.SetCollision(false, false);
                }
                else
                {
                    player.FreezePosition(false);
                    player.SetCollision(true, true);
                }
            }
        }
    }
}