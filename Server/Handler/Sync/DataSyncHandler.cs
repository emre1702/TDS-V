using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Sync
{
    public class DataSyncHandler
    {
        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasAll
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>> _playerHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<PlayerDataKey, object>>>();

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Serializer _serializer;
        private readonly IModAPI _modAPI;

        public DataSyncHandler(EventsHandler eventsHandler, Serializer serializer, IModAPI modAPI)
        {
            _serializer = serializer;
            _modAPI = modAPI;

            eventsHandler.PlayerLoggedIn += SyncPlayerAllData;
            eventsHandler.PlayerJoinedLobby += SyncPlayerLobbyData;

            eventsHandler.PlayerLeftLobby += PlayerLeftLobby;
            eventsHandler.PlayerLoggedOut += PlayerLoggedOut;
        }

        /// <summary>
        /// Only works for default types like int, string etc.!
        /// </summary>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <param name="syncMode"></param>
        /// <param name="value"></param>
        public void SetData(ITDSPlayer player, PlayerDataKey key, PlayerDataSyncMode syncMode, object value)
        {

            switch (syncMode)
            {
                case PlayerDataSyncMode.All:
                    if (!_playerHandleDatasAll.ContainsKey(player.RemoteId))
                        _playerHandleDatasAll[player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasAll[player.RemoteId][key] = value;

                    _modAPI.Sync.SendEvent(ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case PlayerDataSyncMode.Lobby:
                    if (player.Lobby is null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.Lobby.Id))
                        _playerHandleDatasLobby[player.Lobby.Id] = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.Lobby.Id].ContainsKey(player.RemoteId))
                        _playerHandleDatasLobby[player.Lobby.Id][player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasLobby[player.Lobby.Id][player.RemoteId][key] = value;

                    _modAPI.Sync.SendEvent(player.Lobby, ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;

                case PlayerDataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(player.RemoteId))
                        _playerHandleDatasPlayer[player.RemoteId] = new Dictionary<PlayerDataKey, object>();
                    _playerHandleDatasPlayer[player.RemoteId][key] = value;

                    _modAPI.Sync.SendEvent(player, ToClientEvent.SetPlayerData, player.RemoteId, (int)key, value);
                    break;
            }
        }

        private void SyncPlayerAllData(ITDSPlayer player)
        {
            player.SendEvent(ToClientEvent.SyncPlayerData, _serializer.ToClient(_playerHandleDatasAll));
        }

        private void SyncPlayerLobbyData(ITDSPlayer player, ILobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            _modAPI.Sync.SendEvent(player, ToClientEvent.SyncPlayerData, _serializer.ToClient(_playerHandleDatasLobby[lobby.Id]));
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
    }
}
