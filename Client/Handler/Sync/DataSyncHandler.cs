using RAGE.Elements;
using System;
using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Sync
{
    public class DataSyncHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;

        private readonly Dictionary<ushort, Dictionary<EntityDataKey, object>> _entityRemoteIdDatas
            = new Dictionary<ushort, Dictionary<EntityDataKey, object>>();

        private readonly EventsHandler _eventsHandler;

        private readonly Dictionary<ushort, Dictionary<PlayerDataKey, object>> _playerRemoteIdDatas
                                    = new Dictionary<ushort, Dictionary<PlayerDataKey, object>>();

        private readonly Serializer _serializer;
        private bool _nameSyncedWithAngular;

        public DataSyncHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, BrowserHandler angularHandler, Serializer serializer)
                    : base(loggingHandler)
        {
            _browserHandler = angularHandler;
            _serializer = serializer;
            _eventsHandler = eventsHandler;

            eventsHandler.DataChanged += OnLocalPlayerDataChange;

            RAGE.Events.Add(ToClientEvent.SetPlayerData, OnSetPlayerDataMethod);
            RAGE.Events.Add(ToClientEvent.RemoveSyncedPlayerDatas, OnRemoveSyncedPlayerDatasMethod);
            RAGE.Events.Add(ToClientEvent.SyncPlayerData, OnSyncPlayerDataMethod);

            RAGE.Events.Add(ToClientEvent.SetEntityData, OnSetEntityDataMethod);
            RAGE.Events.Add(ToClientEvent.RemoveSyncedEntityDatas, OnRemoveSyncedEntityDatasMethod);
            RAGE.Events.Add(ToClientEvent.SyncEntityData, OnSyncEntityDataMethod);
        }

        public T GetData<T>(ITDSPlayer player, PlayerDataKey key, T returnOnEmpty = default)
        {
            try
            {
                if (!_playerRemoteIdDatas.ContainsKey(player.RemoteId))
                    return returnOnEmpty;
                if (!_playerRemoteIdDatas[player.RemoteId].ContainsKey(key))
                    return returnOnEmpty;

                return (T)Convert.ChangeType(_playerRemoteIdDatas[player.RemoteId][key], typeof(T));
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"Name: {player.Name} | Key {key}");
                return returnOnEmpty;
            }
        }

        public T GetData<T>(Entity entity, EntityDataKey key, T returnOnEmpty = default)
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

        public object GetData(ITDSPlayer player, PlayerDataKey key)
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

        public object GetData(Entity entity, EntityDataKey key)
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
                return GetData(Player.LocalPlayer as ITDSPlayer, key, returnOnEmpty);
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
                return GetData(Player.LocalPlayer as ITDSPlayer, key);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"LocalPlayer | Key {key}");
                return null;
            }
        }

        private void OnLocalPlayerDataChange(ITDSPlayer player, PlayerDataKey key, object obj)
        {
            try
            {
                if (player != Player.LocalPlayer)
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

                    case PlayerDataKey.GangId:
                        _browserHandler.Angular.SyncGangId((int)obj);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex, $"Name: {player.Name} | Key: {key} | Object: {obj}");
            }
        }

        private void OnRemoveSyncedEntityDatasMethod(object[] args)
        {
            try
            {
                ushort entityRemoteId = Convert.ToUInt16(args[0]);
                _entityRemoteIdDatas.Remove(entityRemoteId);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnRemoveSyncedPlayerDatasMethod(object[] args)
        {
            try
            {
                ushort playerRemoteId = Convert.ToUInt16(args[0]);
                _playerRemoteIdDatas.Remove(playerRemoteId);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSetEntityDataMethod(object[] args)
        {
            try
            {
                ushort entityRemoteId = Convert.ToUInt16(args[0]);
                EntityDataKey key = (EntityDataKey)Convert.ToInt32(args[1]);
                object value = args[2];

                if (!_entityRemoteIdDatas.ContainsKey(entityRemoteId))
                    _entityRemoteIdDatas[entityRemoteId] = new Dictionary<EntityDataKey, object>();
                _entityRemoteIdDatas[entityRemoteId][key] = value;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSetPlayerDataMethod(object[] args)
        {
            try
            {
                ushort playerRemoteId = Convert.ToUInt16(args[0]);
                PlayerDataKey key = (PlayerDataKey)Convert.ToInt32(args[1]);
                object value = args[2];

                if (!_playerRemoteIdDatas.ContainsKey(playerRemoteId))
                    _playerRemoteIdDatas[playerRemoteId] = new Dictionary<PlayerDataKey, object>();
                _playerRemoteIdDatas[playerRemoteId][key] = value;

                var player = RAGE.Elements.Entities.Players.GetAtRemote(playerRemoteId) as ITDSPlayer;
                if (player != null)
                {
                    _eventsHandler.OnDataChanged(player, key, value);
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSyncEntityDataMethod(object[] args)
        {
            try
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
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSyncPlayerDataMethod(object[] args)
        {
            try
            {
                string dictJson = (string)args[0];
                var dict = _serializer.FromServer<Dictionary<ushort, Dictionary<PlayerDataKey, object>>>(dictJson);
                foreach (var entry in dict)
                {
                    var player = RAGE.Elements.Entities.Players.GetAtRemote(entry.Key) as ITDSPlayer;
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
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}
