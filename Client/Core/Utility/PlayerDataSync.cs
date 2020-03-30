using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Shared.Enum;
using Player = RAGE.Elements.Player;
using Entities = RAGE.Elements.Entities;
using TDS_Shared.Core;
using TDS_Client.Enum;

namespace TDS_Client.Manager.Utility
{
    static class PlayerDataSync
    {
        public delegate void DataChangedDelegate(Player player, PlayerDataKey key, object data);
        public static event DataChangedDelegate OnDataChanged;

        private static readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerRemoteIdDatas 
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        static PlayerDataSync()
        {
            OnDataChanged += OnLocalPlayerDataChange;
        }

        public static T GetData<T>(Player player, PlayerDataKey key, T returnOnEmpty = default)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return returnOnEmpty;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return returnOnEmpty;

            return (T)_playerRemoteIdDatas[player.RemoteId][key];
        }

        public static object GetData(Player player, PlayerDataKey key)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return null;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return null;

            return _playerRemoteIdDatas[player.RemoteId][key];
        }

        public static T GetData<T>(PlayerDataKey key, T returnOnEmpty = default)
        {
            return GetData<T>(Player.LocalPlayer, key, returnOnEmpty);
        }

        public static object GetData(PlayerDataKey key)
        {
            return GetData(Player.LocalPlayer, key);
        }

        public static void HandleDataFromServer(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            PlayerDataKey key = (PlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                _playerRemoteIdDatas[playerRemoteId] = new Dictionary<PlayerDataKey, object>();
            _playerRemoteIdDatas[playerRemoteId][key] = value;

            var player = Entities.Players.All.FirstOrDefault(p => p.RemoteId == playerRemoteId);
            if (player != null)
            {
                OnDataChanged?.Invoke(player, key, value);
            }
        }

        public static void AppendDictionaryFromServer(string dictJson)
        {
            var dict = Serializer.FromServer<Dictionary<ushort, Dictionary<PlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                var player = Entities.Players.All.FirstOrDefault(p => p.RemoteId == entry.Key);
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

        public static void RemovePlayerData(ushort playerRemoteId)
        {
            _playerRemoteIdDatas.Remove(playerRemoteId);
        }


        private static bool _nameSyncedWithAngular;
        private static void OnLocalPlayerDataChange(Player player, PlayerDataKey key, object obj)
        {
            if (player != Player.LocalPlayer)
                return;
            switch (key)
            {
                case PlayerDataKey.Money:
                    //Stats.StatSetInt(Misc.GetHashKey("SP0_TOTAL_CASH"), (int)obj, false);
                    Browser.Angular.Main.SyncMoney((int)obj);
                    Browser.Angular.Main.SyncHUDDataChange(EHUDDataType.Money, (int)obj);
                    break;
                case PlayerDataKey.AdminLevel:
                    Browser.Angular.Main.RefreshAdminLevel(Convert.ToInt32(obj));
                    break;
                case PlayerDataKey.IsLobbyOwner:
                    Lobby.Lobby.IsLobbyOwner = (bool)obj;
                    break;
                case PlayerDataKey.Name:
                    if (!_nameSyncedWithAngular)
                    {
                        _nameSyncedWithAngular = true;
                        return;
                    }
                    Browser.Angular.Main.SyncUsernameChange((string)obj);
                    break;
            }
        }
    }
}
