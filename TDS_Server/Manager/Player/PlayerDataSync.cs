using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Player
{
    static class PlayerDataSync
    {
        private static readonly Dictionary<ushort, Dictionary<EPlayerDataKey, object>> _playerHandleDatasAll
            = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();

        private static readonly Dictionary<int, Dictionary<ushort, Dictionary<EPlayerDataKey, object>>> _playerHandleDatasLobby
            = new Dictionary<int, Dictionary<ushort, Dictionary<EPlayerDataKey, object>>>();

        private static readonly Dictionary<ushort, Dictionary<EPlayerDataKey, object>> _playerHandleDatasPlayer
            = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();

        static PlayerDataSync()
        {
            CustomEventManager.OnPlayerLoggedIn += SyncPlayerAllData;
            CustomEventManager.OnPlayerJoinedLobby += SyncPlayerLobbyData;

            CustomEventManager.OnPlayerLeftLobby += PlayerLeftLobby;
            CustomEventManager.OnPlayerLoggedOut += PlayerLoggedOut;
        }

        
        /// <summary>
        /// Only works for default types like int, string etc.!
        /// </summary>
        /// <param name="player"></param>
        /// <param name="key"></param>
        /// <param name="syncMode"></param>
        /// <param name="value"></param>
        public static void SetPlayerSyncData(TDSPlayer player, EPlayerDataKey key, EPlayerDataSyncMode syncMode, object value)
        {

            switch (syncMode)
            {
                case EPlayerDataSyncMode.All:
                    if (!_playerHandleDatasAll.ContainsKey(player.Client.Handle.Value))
                        _playerHandleDatasAll[player.Client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasAll[player.Client.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.SetPlayerData, player.Client.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Lobby:
                    if (player.CurrentLobby == null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.CurrentLobby.Id))
                        _playerHandleDatasLobby[player.CurrentLobby.Id] = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.CurrentLobby.Id].ContainsKey(player.Client.Handle.Value))
                        _playerHandleDatasLobby[player.CurrentLobby.Id][player.Client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasLobby[player.CurrentLobby.Id][player.Client.Handle.Value][key] = value;

                    player.CurrentLobby?.SendAllPlayerEvent(DToClientEvent.SetPlayerData, null, player.Client.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(player.Client.Handle.Value))
                        _playerHandleDatasPlayer[player.Client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasPlayer[player.Client.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SetPlayerData, player.Client.Handle.Value, (int)key, value);
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
        public static void SetPlayerSyncData(Client client, EPlayerDataKey key, EPlayerDataSyncMode syncMode, object value)
        {

            switch (syncMode)
            {
                case EPlayerDataSyncMode.All:
                    if (!_playerHandleDatasAll.ContainsKey(client.Handle.Value))
                        _playerHandleDatasAll[client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasAll[client.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.SetPlayerData, client.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Lobby:
                    var player = client.GetChar();
                    if (player.CurrentLobby == null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.CurrentLobby.Id))
                        _playerHandleDatasLobby[player.CurrentLobby.Id] = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.CurrentLobby.Id].ContainsKey(client.Handle.Value))
                        _playerHandleDatasLobby[player.CurrentLobby.Id][client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasLobby[player.CurrentLobby.Id][client.Handle.Value][key] = value;

                    player.CurrentLobby?.SendAllPlayerEvent(DToClientEvent.SetPlayerData, null, client.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(client.Handle.Value))
                        _playerHandleDatasPlayer[client.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasPlayer[client.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SetPlayerData, client.Handle.Value, (int)key, value);
                    break;
            }
        }

        private static void SyncPlayerAllData(TDSPlayer player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncPlayerData, Serializer.ToClient(_playerHandleDatasAll));
        }

        private static void SyncPlayerLobbyData(TDSPlayer player, Lobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncPlayerData, Serializer.ToClient(_playerHandleDatasLobby[lobby.Id]));
        }

        private static void PlayerLeftLobby(TDSPlayer player, Lobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            if (!_playerHandleDatasLobby[lobby.Id].ContainsKey(player.Client.Handle.Value))
                return;

            _playerHandleDatasLobby[lobby.Id].Remove(player.Client.Handle.Value);
        }

        private static void PlayerLoggedOut(TDSPlayer player)
        {
            if (_playerHandleDatasAll.ContainsKey(player.Client.Handle.Value))
                _playerHandleDatasAll.Remove(player.Client.Handle.Value);

            if (_playerHandleDatasPlayer.ContainsKey(player.Client.Handle.Value))
                _playerHandleDatasPlayer.Remove(player.Client.Handle.Value);

            NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.RemoveSyncedPlayerDatas, player.Client.Handle.Value);
        }
    }
}
