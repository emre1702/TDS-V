using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using Player = RAGE.Elements.Player;
using Entities = RAGE.Elements.Entities;
using TDS_Common.Manager.Utility;
using TDS_Client.Enum;

namespace TDS_Client.Manager.Utility
{
    static class PlayerDataSync
    {
        public delegate void DataChangedDelegate(Player player, EPlayerDataKey key, object data);
        public static event DataChangedDelegate OnDataChanged;

        private static readonly Dictionary<ushort, Dictionary<EPlayerDataKey, object>> _playerRemoteIdDatas 
            = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();

        static PlayerDataSync()
        {
            OnDataChanged += OnLocalPlayerDataChange;
        }

        public static T GetData<T>(Player player, EPlayerDataKey key, T returnOnEmpty = default)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return returnOnEmpty;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return returnOnEmpty;

            return (T)_playerRemoteIdDatas[player.RemoteId][key];
        }

        public static object GetData(Player player, EPlayerDataKey key)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return null;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return null;

            return _playerRemoteIdDatas[player.RemoteId][key];
        }

        public static T GetData<T>(EPlayerDataKey key, T returnOnEmpty = default)
        {
            return GetData<T>(Player.LocalPlayer, key, returnOnEmpty);
        }

        public static object GetData(EPlayerDataKey key)
        {
            return GetData(Player.LocalPlayer, key);
        }

        public static void HandleDataFromServer(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            EPlayerDataKey key = (EPlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                _playerRemoteIdDatas[playerRemoteId] = new Dictionary<EPlayerDataKey, object>();
            _playerRemoteIdDatas[playerRemoteId][key] = value;

            var player = Entities.Players.All.FirstOrDefault(p => p.RemoteId == playerRemoteId);
            if (player != null)
            {
                OnDataChanged?.Invoke(player, key, value);
            }
        }

        public static void AppendDictionaryFromServer(string dictJson)
        {
            var dict = Serializer.FromServer<Dictionary<ushort, Dictionary<EPlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                var player = Entities.Players.All.FirstOrDefault(p => p.RemoteId == entry.Key);
                if (!_playerRemoteIdDatas.ContainsKey(entry.Key))
                    _playerRemoteIdDatas[entry.Key] = new Dictionary<EPlayerDataKey, object>();
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


        private static void OnLocalPlayerDataChange(Player player, EPlayerDataKey key, object obj)
        {
            if (player != Player.LocalPlayer)
                return;
            switch (key)
            {
                case EPlayerDataKey.Money:
                    //Stats.StatSetInt(Misc.GetHashKey("SP0_TOTAL_CASH"), (int)obj, false);
                    Browser.Angular.Main.SyncMoney((int)obj);
                    Browser.Angular.Main.SyncHUDDataChange(EHUDDataType.Money, (int)obj);
                    break;
                case EPlayerDataKey.AdminLevel:
                    Browser.Angular.Main.RefreshAdminLevel(Convert.ToInt32(obj));
                    break;
                case EPlayerDataKey.IsLobbyOwner:
                    Lobby.Lobby.IsLobbyOwner = (bool)obj;
                    break;
            }
        }
    }
}
