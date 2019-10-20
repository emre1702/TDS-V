using Newtonsoft.Json;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class PlayerDataSync
    {
        private static readonly Dictionary<ushort, Dictionary<EPlayerDataKey, object>> _playerHandleDatas 
            = new Dictionary<ushort, Dictionary<EPlayerDataKey, object>>();

        public static T GetData<T>(Player player, EPlayerDataKey key)
        {
            if (!_playerHandleDatas.ContainsKey(player.RemoteId))
                return default;
            if (!_playerHandleDatas[player.RemoteId].ContainsKey(key))
                return default;

            return (T)_playerHandleDatas[player.RemoteId][key];
        }

        public static object GetData(Player player, EPlayerDataKey key)
        {
            if (!_playerHandleDatas.ContainsKey(player.RemoteId))
                return null;
            if (!_playerHandleDatas[player.RemoteId].ContainsKey(key))
                return null;

            return _playerHandleDatas[player.RemoteId][key];
        }

        public static void HandleDataFromServer(object[] args)
        {
            ushort playerHandle = Convert.ToUInt16(args[0]);
            EPlayerDataKey key = (EPlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerHandleDatas.ContainsKey(playerHandle))
                _playerHandleDatas[playerHandle] = new Dictionary<EPlayerDataKey, object>();
            _playerHandleDatas[playerHandle][key] = value;
        }

        public static void AppendDictionaryFromServer(string dictJson)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<ushort, Dictionary<EPlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                if (!_playerHandleDatas.ContainsKey(entry.Key))
                    _playerHandleDatas[entry.Key] = new Dictionary<EPlayerDataKey, object>();
                foreach (var dataEntry in entry.Value)
                {
                    _playerHandleDatas[entry.Key][dataEntry.Key] = dataEntry.Value;
                }
            }
        }

        public static void RemovePlayerData(ushort playerHandle)
        {
            _playerHandleDatas.Remove(playerHandle);
        }
    }
}
