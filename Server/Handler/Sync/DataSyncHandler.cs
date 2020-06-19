using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Sync
{
    public class DataSyncHandler
    {
        #region Private Fields

        private readonly Dictionary<ushort, Dictionary<EntityDataKey, object>> _entityHandleDatasAll
            = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();

        private readonly Dictionary<int, Dictionary<ushort, Dictionary<EntityDataKey, object>>> _entityHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<EntityDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<ushort, Dictionary<EntityDataKey, object>>> _entityHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<ushort, Dictionary<EntityDataKey, object>>>();

        private readonly IModAPI _modAPI;

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasAll
                                            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>> _playerHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public DataSyncHandler(EventsHandler eventsHandler, Serializer serializer, IModAPI modAPI)
        {
            _serializer = serializer;
            _modAPI = modAPI;

            eventsHandler.PlayerLoggedIn += SyncPlayerAllData;
            eventsHandler.PlayerJoinedLobby += SyncPlayerLobbyData;

            eventsHandler.PlayerLeftLobby += PlayerLeftLobby;
            eventsHandler.PlayerLoggedOut += PlayerLoggedOut;
            eventsHandler.EntityDeleted += EntityDeleted;
        }

        #endregion Public Constructors

        #region Public Methods

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

                    _modAPI.Sync.SendEvent(ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case DataSyncMode.Lobby:
                    if (player.Lobby is null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.Lobby.Id))
                        _playerHandleDatasLobby[player.Lobby.Id] = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.Lobby.Id].ContainsKey(player.RemoteId))
                        _playerHandleDatasLobby[player.Lobby.Id][player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasLobby[player.Lobby.Id][player.RemoteId][key] = value;

                    _modAPI.Sync.SendEvent(player.Lobby, ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case DataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(player.RemoteId))
                        _playerHandleDatasPlayer[player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasPlayer[player.RemoteId][key] = value;

                    _modAPI.Sync.SendEvent(player, ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
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
        public void SetData(IEntity entity, EntityDataKey key, DataSyncMode syncMode, object value, ITDSPlayer? toPlayer = null, ILobby? toLobby = null)
        {
            switch (syncMode)
            {
                case DataSyncMode.All:
                    if (!_entityHandleDatasAll.ContainsKey(entity.Id))
                        _entityHandleDatasAll[entity.Id] = new Dictionary<EntityDataKey, object>();
                    _entityHandleDatasAll[entity.Id][key] = value;

                    _modAPI.Sync.SendEvent(ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;

                case DataSyncMode.Lobby:
                    if (toLobby is null)
                        return;

                    if (!_entityHandleDatasLobby.ContainsKey(toLobby.Id))
                        _entityHandleDatasLobby[toLobby.Id] = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();
                    if (!_entityHandleDatasLobby[toLobby.Id].ContainsKey(entity.Id))
                        _entityHandleDatasLobby[toLobby.Id][entity.Id] = new Dictionary<EntityDataKey, object>();
                    _entityHandleDatasLobby[toLobby.Id][entity.Id][key] = value;

                    _modAPI.Sync.SendEvent(toLobby, ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;

                case DataSyncMode.Player:
                    if (toPlayer is null)
                        return;

                    if (!_entityHandleDatasPlayer.ContainsKey(toPlayer.RemoteId))
                        _entityHandleDatasPlayer[toPlayer.RemoteId] = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();
                    if (!_entityHandleDatasPlayer[toPlayer.RemoteId].ContainsKey(entity.Id))
                        _entityHandleDatasPlayer[toPlayer.RemoteId][entity.Id] = new Dictionary<EntityDataKey, object>();

                    _entityHandleDatasPlayer[toPlayer.RemoteId][entity.Id][key] = value;

                    toPlayer.SendEvent(ToClientEvent.SetPlayerData, entity.Id, (int)key, value);
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void EntityDeleted(IEntity entity)
        {
            _modAPI.Sync.SendEvent(ToClientEvent.RemoveSyncedEntityDatas, entity.RemoteId);
        }

        private void PlayerLeftLobby(ITDSPlayer player, ILobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            if (!_playerHandleDatasLobby[lobby.Id].ContainsKey(player.RemoteId))
                return;

            _playerHandleDatasLobby[lobby.Id].Remove(player.RemoteId);
        }

        private void PlayerLoggedOut(ITDSPlayer player)
        {
            if (_playerHandleDatasAll.ContainsKey(player.RemoteId))
                _playerHandleDatasAll.Remove(player.RemoteId);

            if (_playerHandleDatasPlayer.ContainsKey(player.RemoteId))
                _playerHandleDatasPlayer.Remove(player.RemoteId);

            _modAPI.Sync.SendEvent(ToClientEvent.RemoveSyncedPlayerDatas, player.RemoteId);
        }

        private void SyncPlayerAllData(ITDSPlayer player)
        {
            player.SendEvent(ToClientEvent.SyncPlayerData, _serializer.ToClient(_playerHandleDatasAll));
            player.SendEvent(ToClientEvent.SyncEntityData, _serializer.ToClient(_entityHandleDatasAll));
        }

        private void SyncPlayerLobbyData(ITDSPlayer player, ILobby lobby)
        {
            if (_playerHandleDatasLobby.ContainsKey(lobby.Id))
                _modAPI.Sync.SendEvent(player, ToClientEvent.SyncPlayerData, _serializer.ToClient(_playerHandleDatasLobby[lobby.Id]));

            if (_entityHandleDatasLobby.ContainsKey(lobby.Id))
                _modAPI.Sync.SendEvent(player, ToClientEvent.SyncEntityData, _serializer.ToClient(_entityHandleDatasLobby[lobby.Id]));
        }

        #endregion Private Methods
    }
}
