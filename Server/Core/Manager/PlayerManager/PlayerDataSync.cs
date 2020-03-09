using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Enums;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Manager.PlayerManager
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
        public static void SetData(TDSPlayer player, EPlayerDataKey key, EPlayerDataSyncMode syncMode, object value)
        {
            if (player.Player is null)
                return;

            switch (syncMode)
            {
                case EPlayerDataSyncMode.All:
                    if (!_playerHandleDatasAll.ContainsKey(player.Player.Handle.Value))
                        _playerHandleDatasAll[player.Player.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasAll[player.Player.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.SetPlayerData, player.Player.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Lobby:
                    if (player.CurrentLobby == null)
                        return;

                    if (!_playerHandleDatasLobby.ContainsKey(player.CurrentLobby.Id))
                        _playerHandleDatasLobby[player.CurrentLobby.Id] = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();
                    if (!_playerHandleDatasLobby[player.CurrentLobby.Id].ContainsKey(player.Player.Handle.Value))
                        _playerHandleDatasLobby[player.CurrentLobby.Id][player.Player.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasLobby[player.CurrentLobby.Id][player.Player.Handle.Value][key] = value;

                    player.CurrentLobby?.SendAllPlayerEvent(DToClientEvent.SetPlayerData, null, player.Player.Handle.Value, (int)key, value);
                    break;
                case EPlayerDataSyncMode.Player:
                    if (!_playerHandleDatasPlayer.ContainsKey(player.Player.Handle.Value))
                        _playerHandleDatasPlayer[player.Player.Handle.Value] = new Dictionary<EPlayerDataKey, object>();
                    _playerHandleDatasPlayer[player.Player.Handle.Value][key] = value;

                    NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SetPlayerData, player.Player.Handle.Value, (int)key, value);
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
        public static void SetData(Player client, EPlayerDataKey key, EPlayerDataSyncMode syncMode, object value)
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
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SyncPlayerData, Serializer.ToClient(_playerHandleDatasAll));
        }

        private static void SyncPlayerLobbyData(TDSPlayer player, Lobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SyncPlayerData, Serializer.ToClient(_playerHandleDatasLobby[lobby.Id]));
        }

        private static void PlayerLeftLobby(TDSPlayer player, Lobby lobby)
        {
            if (!_playerHandleDatasLobby.ContainsKey(lobby.Id))
                return;
            if (!_playerHandleDatasLobby[lobby.Id].ContainsKey(player.Player!.Handle.Value))
                return;

            _playerHandleDatasLobby[lobby.Id].Remove(player.Player.Handle.Value);
        }

        private static void PlayerLoggedOut(TDSPlayer player)
        {
            if (_playerHandleDatasAll.ContainsKey(player.Player!.Handle.Value))
                _playerHandleDatasAll.Remove(player.Player.Handle.Value);

            if (_playerHandleDatasPlayer.ContainsKey(player.Player.Handle.Value))
                _playerHandleDatasPlayer.Remove(player.Player.Handle.Value);

            NAPI.ClientEvent.TriggerClientEventForAll(DToClientEvent.RemoveSyncedPlayerDatas, player.Player.Handle.Value);
        }
    }
}
