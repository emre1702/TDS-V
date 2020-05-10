using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Sync
{
    public class DataSyncHandler : ServiceBase
    {
        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerRemoteIdDatas
            = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();
        private readonly Dictionary<ushort, Dictionary<EntityDataKey, object>> _entityRemoteIdDatas
            = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();

        private readonly BrowserHandler _browserHandler;
        private readonly Serializer _serializer;
        private readonly EventsHandler _eventsHandler;

        public DataSyncHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler, BrowserHandler angularHandler, Serializer serializer)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = angularHandler;
            _serializer = serializer;
            _eventsHandler = eventsHandler;

            eventsHandler.DataChanged += OnLocalPlayerDataChange;

            modAPI.Event.Add(ToClientEvent.SetPlayerData, OnSetPlayerDataMethod);
            modAPI.Event.Add(ToClientEvent.RemoveSyncedPlayerDatas, OnRemoveSyncedPlayerDatasMethod);
            modAPI.Event.Add(ToClientEvent.SyncPlayerData, OnSyncPlayerDataMethod);

            modAPI.Event.Add(ToClientEvent.SetEntityData, OnSetEntityDataMethod);
            modAPI.Event.Add(ToClientEvent.RemoveSyncedEntityDatas, OnRemoveSyncedEntityDatasMethod);
            modAPI.Event.Add(ToClientEvent.SyncEntityData, OnSyncEntityDataMethod);
        }

        public T GetData<T>(IPlayer player, PlayerDataKey key, T returnOnEmpty = default)
        {
            try
            {
                if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                    return returnOnEmpty;
                if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                    return returnOnEmpty;

                return (T)_playerRemoteIdDatas[player.RemoteId][key];
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"Name: {player.Name} | Key {key}");
                return returnOnEmpty;
            }
        }

        public T GetData<T>(IEntity entity, EntityDataKey key, T returnOnEmpty = default)
        {
            try 
            { 
                if (!_entityRemoteIdDatas.ContainsKey(entity.RemoteId))
                    return returnOnEmpty;
                if (!_entityRemoteIdDatas[entity.RemoteId].ContainsKey(key))
                    return returnOnEmpty;

                return (T)_entityRemoteIdDatas[entity.RemoteId][key];
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"IsEntity | Key {key}");
                return returnOnEmpty;
            }
        }

        public object GetData(IPlayer player, PlayerDataKey key)
        {
            try
            {
                if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                    return null;
                if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                    return null;

                return _playerRemoteIdDatas[player.RemoteId][key];
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"Name: {player.Name} | Key {key}");
                return null;
            }
        }

        public object GetData(IEntity entity, EntityDataKey key)
        {
            try
            {
                if (!_entityRemoteIdDatas.ContainsKey(entity.RemoteId))
                    return null;
                if (!_entityRemoteIdDatas[entity.RemoteId].ContainsKey(key))
                    return null;

                return _entityRemoteIdDatas[entity.RemoteId][key];
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"IsEntity | Key {key}");
                return null;
            }
        }

        public T GetData<T>(PlayerDataKey key, T returnOnEmpty = default)
        {
            try
            {
                return GetData(ModAPI.LocalPlayer, key, returnOnEmpty);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"LocalPlayer | Key {key}");
                return returnOnEmpty;
            }
        }

        public object GetData(PlayerDataKey key)
        {
            try
            {
                return GetData(ModAPI.LocalPlayer, key);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"LocalPlayer | Key {key}");
                return null;
            }
        }

        private void OnSetPlayerDataMethod(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            PlayerDataKey key = (PlayerDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                _playerRemoteIdDatas[playerRemoteId] = new Dictionary<PlayerDataKey, object>();
            _playerRemoteIdDatas[playerRemoteId][key] = value;

            var player = ModAPI.Pool.Players.GetAtRemote(playerRemoteId);
            if (player != null)
            {
                _eventsHandler.OnDataChanged(player, key, value);
            }
        }

        private void OnSetEntityDataMethod(object[] args)
        {
            ushort entityRemoteId = Convert.ToUInt16(args[0]);
            EntityDataKey key = (EntityDataKey)Convert.ToInt32(args[1]);
            object value = args[2];

            if (!_entityRemoteIdDatas.ContainsKey(entityRemoteId))
                _entityRemoteIdDatas[entityRemoteId] = new Dictionary<EntityDataKey, object>();
            _entityRemoteIdDatas[entityRemoteId][key] = value;
        }

        private void OnSyncPlayerDataMethod(object[] args)
        {
            string dictJson = (string)args[0];
            var dict = _serializer.FromServer<Dictionary<ushort, Dictionary<PlayerDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                var player = ModAPI.Pool.Players.GetAtRemote(entry.Key);
                if (!_playerRemoteIdDatas.ContainsKey(entry.Key))
                    _playerRemoteIdDatas[entry.Key] = new Dictionary<PlayerDataKey, object>();
                foreach (var dataEntry in entry.Value)
                {
                    _playerRemoteIdDatas[entry.Key][dataEntry.Key] = dataEntry.Value;
                    if (player != null)
                    {
                        _eventsHandler.OnDataChanged(player, dataEntry.Key, dataEntry.Value);
                    }
                }
            }
        }

        private void OnSyncEntityDataMethod(object[] args)
        {
            string dictJson = (string)args[0];
            var dict = _serializer.FromServer<Dictionary<ushort, Dictionary<EntityDataKey, object>>>(dictJson);
            foreach (var entry in dict)
            {
                if (!_entityRemoteIdDatas.ContainsKey(entry.Key))
                    _entityRemoteIdDatas[entry.Key] = new Dictionary<EntityDataKey, object>();
                foreach (var dataEntry in entry.Value)
                {
                    _entityRemoteIdDatas[entry.Key][dataEntry.Key] = dataEntry.Value;
                }
            }
        }

        private void OnRemoveSyncedPlayerDatasMethod(object[] args)
        {
            ushort playerRemoteId = Convert.ToUInt16(args[0]);
            _playerRemoteIdDatas.Remove(playerRemoteId);
        }

        private void OnRemoveSyncedEntityDatasMethod(object[] args)
        {
            ushort entityRemoteId = Convert.ToUInt16(args[0]);
            _entityRemoteIdDatas.Remove(entityRemoteId);
        }

        private bool _nameSyncedWithAngular;
        private void OnLocalPlayerDataChange(IPlayer player, PlayerDataKey key, object obj)
        {
            if (player != ModAPI.LocalPlayer)
                return;
            switch (key)
            {
                case PlayerDataKey.Money:
                    //Stats.StatSetInt(Misc.GetHashKey("SP0_TOTAL_CASH"), (int)obj, false);
                    _browserHandler.Angular.SyncMoney((int)obj);
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.Money, (int)obj);
                    break;
                case PlayerDataKey.AdminLevel:
                    _browserHandler.Angular.RefreshAdminLevel(Convert.ToInt32(obj));
                    break;
                case PlayerDataKey.Name:
                    if (!_nameSyncedWithAngular)
                    {
                        _nameSyncedWithAngular = true;
                        return;
                    }
                    _browserHandler.Angular.SyncUsernameChange((string)obj);
                    break;
            }
        }
    }
}
