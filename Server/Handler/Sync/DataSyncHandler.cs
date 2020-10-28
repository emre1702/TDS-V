using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Sync
{
    public class DataSyncHandler
    {
        private readonly Dictionary<ushort, Dictionary<EntityDataKey, object>> _entityHandleDatasAll
            = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();

        private readonly Dictionary<int, Dictionary<ushort, Dictionary<EntityDataKey, object>>> _entityHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<EntityDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<ushort, Dictionary<EntityDataKey, object>>> _entityHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<ushort, Dictionary<EntityDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasAll
                                            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>> _playerHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        public DataSyncHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerLoggedIn += SyncPlayerAllData;
            eventsHandler.PlayerJoinedLobby += SyncPlayerLobbyData;

            eventsHandler.PlayerLeftLobby += PlayerLeftLobby;
            eventsHandler.PlayerLoggedOut += PlayerLoggedOut;
            eventsHandler.EntityDeleted += EntityDeleted;
        }

        /// <summary>
        /// Only works for default types like int, string etc.!
        /// </summary>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <param name="syncMode"></param>
        /// <param name="value"></param>
        public void SetData(ITDSPlayer player, PlayerDataKey key, DataSyncMode syncMode, object value)
        {
            switch (syncMode)
            {
                case DataSyncMode.All:
                    if (!_playerHandleDatasAll.ContainsKey(player.RemoteId))
                        _playerHandleDatasAll[player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasAll[player.RemoteId][key] = value;

                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case DataSyncMode.Lobby:
                    if (player.Lobby is null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.Lobby.Entity.Id))
                        _playerHandleDatasLobby[player.Lobby.Entity.Id] = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.Lobby.Entity.Id].ContainsKey(player.RemoteId))
                        _playerHandleDatasLobby[player.Lobby.Entity.Id][player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasLobby[player.Lobby.Entity.Id][player.RemoteId][key] = value;

                    player.Lobby.Sync.TriggerEvent(ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case DataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(player.RemoteId))
                        _playerHandleDatasPlayer[player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasPlayer[player.RemoteId][key] = value;

                    player.TriggerEvent(ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;
            }
        }

        /// <summary>
        /// Only works for default types like int, string etc.!
        /// </summary>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <param name="syncMode"></param>
        /// <param name="value"></param>
        public void SetData(Entity entity, EntityDataKey key, DataSyncMode syncMode, object value, ITDSPlayer? toPlayer = null, Data.Interfaces.LobbySystem.Lobbies.Abstracts.IBaseLobby? toLobby = null)
        {
            switch (syncMode)
            {
                case DataSyncMode.All:
                    if (!_entityHandleDatasAll.ContainsKey(entity.Id))
                        _entityHandleDatasAll[entity.Id] = new Dictionary<EntityDataKey, object>();
                    _entityHandleDatasAll[entity.Id][key] = value;

                    NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;

                case DataSyncMode.Lobby:
                    if (toLobby is null)
                        return;

                    if (!_entityHandleDatasLobby.ContainsKey(toLobby.Entity.Id))
                        _entityHandleDatasLobby[toLobby.Entity.Id] = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();
                    if (!_entityHandleDatasLobby[toLobby.Entity.Id].ContainsKey(entity.Id))
                        _entityHandleDatasLobby[toLobby.Entity.Id][entity.Id] = new Dictionary<EntityDataKey, object>();
                    _entityHandleDatasLobby[toLobby.Entity.Id][entity.Id][key] = value;

                    toLobby.Sync.TriggerEvent(ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;

                case DataSyncMode.Player:
                    if (toPlayer is null)
                        return;

                    if (!_entityHandleDatasPlayer.ContainsKey(toPlayer.RemoteId))
                        _entityHandleDatasPlayer[toPlayer.RemoteId] = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();
                    if (!_entityHandleDatasPlayer[toPlayer.RemoteId].ContainsKey(entity.Id))
                        _entityHandleDatasPlayer[toPlayer.RemoteId][entity.Id] = new Dictionary<EntityDataKey, object>();

                    _entityHandleDatasPlayer[toPlayer.RemoteId][entity.Id][key] = value;

                    toPlayer.TriggerEvent(ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;
            }
        }

        private void EntityDeleted(Entity entity)
        {
            NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.RemoveSyncedEntityDatas, entity.Handle.Value);
        }

        private void PlayerLeftLobby(ITDSPlayer player, IBaseLobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Entity.Id))
                return;
            if (!_playerHandleDatasLobby[lobby.Entity.Id].ContainsKey(player.RemoteId))
                return;

            _playerHandleDatasLobby[lobby.Entity.Id].Remove(player.RemoteId);
        }

        private void PlayerLoggedOut(ITDSPlayer player)
        {
            if (_playerHandleDatasAll.ContainsKey(player.RemoteId))
                _playerHandleDatasAll.Remove(player.RemoteId);

            if (_playerHandleDatasPlayer.ContainsKey(player.RemoteId))
                _playerHandleDatasPlayer.Remove(player.RemoteId);

            NAPI.ClientEvent.TriggerClientEventForAll(ToClientEvent.RemoveSyncedPlayerDatas, player.RemoteId);
        }

        private void SyncPlayerAllData(ITDSPlayer player)
        {
            var playerHandleDatasAllJson = Serializer.ToClient(_playerHandleDatasAll);
            var entityHandleDatasAllJson = Serializer.ToClient(_entityHandleDatasAll);

            NAPI.Task.RunSafe(() =>
            {
                player.TriggerEvent(ToClientEvent.SyncPlayerData, playerHandleDatasAllJson);
                player.TriggerEvent(ToClientEvent.SyncEntityData, entityHandleDatasAllJson);
            });
        }

        private void SyncPlayerLobbyData(ITDSPlayer player, IBaseLobby lobby)
        {
            if (_playerHandleDatasLobby.ContainsKey(lobby.Entity.Id))
                player.TriggerEvent(ToClientEvent.SyncPlayerData, Serializer.ToClient(_playerHandleDatasLobby[lobby.Entity.Id]));

            if (_entityHandleDatasLobby.ContainsKey(lobby.Entity.Id))
                player.TriggerEvent(ToClientEvent.SyncEntityData, Serializer.ToClient(_entityHandleDatasLobby[lobby.Entity.Id]));
        }
    }
}
