using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Sync
{
    public class DataSyncHandler
    {
        public delegate void DataChangedDelegate(IPlayer player, PlayerDataKey key, object data);
        public static event DataChangedDelegate OnDataChanged;

        private static readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerRemoteIdDatas
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _angularHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly Serializer _serializer;

        public DataSyncHandler(EventsHandler eventsHandler, IModAPI modAPI, BrowserHandler angularHandler, LobbyHandler lobbyHandler, Serializer serializer)
        {
            _modAPI = modAPI;
            _angularHandler = angularHandler;
            _lobbyHandler = lobbyHandler;
            _serializer = serializer;

            OnDataChanged += OnLocalPlayerDataChange;
        }

        public T GetData<T>(IPlayer player, PlayerDataKey key, T returnOnEmpty = default)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return returnOnEmpty;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return returnOnEmpty;

            return (T)_playerRemoteIdDatas[player.RemoteId][key];
        }

        public object GetData(IPlayer player, PlayerDataKey key)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return null;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return null;

            return _playerRemoteIdDatas[player.RemoteId][key];
        }

        public T GetData<T>(PlayerDataKey key, T returnOnEmpty = default)
        {
            return GetData<T>(_modAPI.LocalPlayer, key, returnOnEmpty);
        }

        public object GetData(PlayerDataKey key)
        {
            return GetData(_modAPI.LocalPlayer, key);
        }

        public void HandleDataFromServer(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            PlayerDataKey key = (PlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                _playerRemoteIdDatas[playerRemoteId] = new Dictionary<PlayerDataKey, object>();
            _playerRemoteIdDatas[playerRemoteId][key] = value;

            var player = _modAPI.Pool.Players.GetAtRemote(playerRemoteId);
            if (player != null)
            {
                OnDataChanged?.Invoke(player, key, value);
            }
        }

        public void AppendDictionaryFromServer(string dictJson)
        {
            var dict = _serializer.FromServer<Dictionary<ushort, Dictionary<PlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                var player = _modAPI.Pool.Players.GetAtRemote(entry.Key);
                if (!_playerRemoteIdDatas.ContainsKey(entry.Key))
                    _playerRemoteIdDatas[entry.Key] = new Dictionary<PlayerDataKey, object>();
                foreach (var dataEntry in entry.Value)
                {
                    _playerRemoteIdDatas[entry.Key][dataEntry.Key] = dataEntry.Value;
                    if (player != null)
                    {
                        OnDataChanged?.Invoke(player, dataEntry.Key, dataEntry.Value);
                    }
                }
            }
        }

        public void RemovePlayerData(ushort playerRemoteId)
        {
            _playerRemoteIdDatas.Remove(playerRemoteId);
        }


        private bool _nameSyncedWithAngular;
        private void OnLocalPlayerDataChange(IPlayer player, PlayerDataKey key, object obj)
        {
            if (player != _modAPI.LocalPlayer)
                return;
            switch (key)
            {
                case PlayerDataKey.Money:
                    //Stats.StatSetInt(Misc.GetHashKey("SP0_TOTAL_CASH"), (int)obj, false);
                    _angularHandler.Main.SyncMoney((int)obj);
                    _angularHandler.Main.SyncHUDDataChange(HudDataType.Money, (int)obj);
                    break;
                case PlayerDataKey.AdminLevel:
                    _angularHandler.Main.RefreshAdminLevel(Convert.ToInt32(obj));
                    break;
                case PlayerDataKey.IsLobbyOwner:
                    _lobbyHandler.IsLobbyOwner = (bool)obj;
                    break;
                case PlayerDataKey.Name:
                    if (!_nameSyncedWithAngular)
                    {
                        _nameSyncedWithAngular = true;
                        return;
                    }
                    _angularHandler.Main.SyncUsernameChange((string)obj);
                    break;
            }
        }
    }
}
