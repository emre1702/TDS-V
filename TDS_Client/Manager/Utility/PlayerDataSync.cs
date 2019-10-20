using Newtonsoft.Json;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class PlayerDataSync
    {
        private static readonly Dictionary<ushort, Dictionary<EPlayerDataKey, object>> _playerRemoteIdDatas 
            = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();

        public static T GetData<T>(Player player, EPlayerDataKey key)
        {
            if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                return default;
            if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                return default;

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

        public static void HandleDataFromServer(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            EPlayerDataKey key = (EPlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                _playerRemoteIdDatas[playerRemoteId] = new Dictionary<EPlayerDataKey, object>();
            _playerRemoteIdDatas[playerRemoteId][key] = value;

            if (playerRemoteId == Player.LocalPlayer.RemoteId)
                Event.EventsHandler.OnLocalPlayerDataChange(key, value);
        }

        public static void AppendDictionaryFromServer(string dictJson)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<ushort, Dictionary<EPlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                if (!_playerRemoteIdDatas.ContainsKey(entry.Key))
                    _playerRemoteIdDatas[entry.Key] = new Dictionary<EPlayerDataKey, object>();
                foreach (var dataEntry in entry.Value)
                {
                    _playerRemoteIdDatas[entry.Key][dataEntry.Key] = dataEntry.Value;
                }
            }
        }

        public static void RemovePlayerData(ushort playerRemoteId)
        {
            _playerRemoteIdDatas.Remove(playerRemoteId);
        }
    }
}
