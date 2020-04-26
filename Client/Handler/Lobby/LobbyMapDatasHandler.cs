using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Map;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyMapDatasHandler : ServiceBase
    {
        public ClientSyncedDataDto MapDatas { get; set; }

        private DxText _mapInfo;
        private readonly List<IEntityBase> _objects = new List<IEntityBase>();

        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly LobbyCamHandler _lobbyCamHandler;
        private readonly MapLimitHandler _mapLimitHandler;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;
        private readonly EventsHandler _eventsHandler;

        public LobbyMapDatasHandler(IModAPI modAPI, LoggingHandler loggingHandler, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler, 
            LobbyCamHandler lobbyCamHandler, MapLimitHandler mapLimitHandler, Serializer serializer, SettingsHandler settingsHandler)
            : base(modAPI, loggingHandler)
        {
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _lobbyCamHandler = lobbyCamHandler;
            _mapLimitHandler = mapLimitHandler;
            _serializer = serializer;
            _settingsHandler = settingsHandler;
            _eventsHandler = eventsHandler;

            eventsHandler.LobbyLeft += CustomEventManager_OnLobbyLeave;

            modAPI.Event.Add(ToClientEvent.MapChange, OnMapChangeMethod);
            modAPI.Event.Add(ToClientEvent.MapClear, OnMapClearMethod);
        }

        public void SetMapData(ClientSyncedDataDto mapData)
        {
            try
            {
                Logging.LogInfo("", "LobbyMapDatasHandler.SetMapData");
                MapDatas = mapData;

                if (_mapInfo == null)
                    _mapInfo = new DxText(_dxHandler, ModAPI, _timerHandler, MapDatas.Name, 0.01f, 0.995f, 0.2f, Color.White, alignmentX: AlignmentX.Left, alignmentY: AlignmentY.Bottom);
                else
                    _mapInfo.Text = MapDatas.Name;

                LoadMap(mapData);

                if (mapData.Target != null)
                    _lobbyCamHandler.SetToMapCenter(mapData.Target);
                else if (mapData.Center != null)
                    _lobbyCamHandler.SetToMapCenter(mapData.Center);

                if (mapData.MapEdges != null && mapData.MapEdges.Count > 0)
                    _mapLimitHandler.Load(mapData.MapEdges);
                Logging.LogInfo("", "LobbyMapDatasHandler.SetMapData", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void CustomEventManager_OnLobbyLeave(SyncedLobbySettings settings)
        {
            OnMapClearMethod(Array.Empty<object>());
            MapDatas = null;
            RemoveMapInfo();
        }

        public void RemoveMapInfo()
        {
            _mapInfo?.Remove();
            _mapInfo = null;
        }

        private void LoadMap(ClientSyncedDataDto map)
        {
            try
            {
                OnMapClearMethod(Array.Empty<object>());

                if (map.Target != null)
                {
                    var obj = ModAPI.MapObject.Create(ModAPI.Misc.GetHashKey(Constants.TargetHashName), map.Target, new Position3D(), dimension: ModAPI.LocalPlayer.Dimension);
                    obj.FreezePosition(true);
                    obj.SetCollision(false, true);
                    obj.SetInvincible(true);
                    _objects.Add(obj);
                }

                if (map.Objects != null)
                {
                    foreach (var data in map.Objects)
                    {
                        string objName = Convert.ToString(data.Info);
                        uint objectHash = ModAPI.Misc.GetHashKey(objName);
                        var obj = ModAPI.MapObject.Create(objectHash, GetPos(data), GetRot(data), dimension: ModAPI.LocalPlayer.Dimension);
                        obj.FreezePosition(true);
                        obj.SetInvincible(true);
                        _objects.Add(obj);
                    }
                }

                if (map.BombPlaces != null)
                {
                    foreach (var data in map.BombPlaces)
                    {
                        var bombPlantHash = ModAPI.Misc.GetHashKey(Constants.BombPlantPlaceHashName);
                        var obj = ModAPI.MapObject.Create(bombPlantHash, data, new Position3D(), dimension: ModAPI.LocalPlayer.Dimension);
                        obj.FreezePosition(true);
                        obj.SetInvincible(true);
                        _objects.Add(obj);
                    }
                }

                if (map.Vehicles != null)
                {
                    foreach (var data in map.Vehicles)
                    {
                        string vehName = Convert.ToString(data.Info);
                        uint vehHash = ModAPI.Misc.GetHashKey(vehName);
                        var veh = ModAPI.Vehicle.Create(vehHash, GetPos(data), GetRot(data), map.Name, locked: true, dimension: ModAPI.LocalPlayer.Dimension);
                        veh.FreezePosition(true);
                        veh.SetInvincible(true);
                        _objects.Add(veh);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private Position3D GetPos(MapCreatorPosition pos)
        {
            return new Position3D(pos.PosX, pos.PosY, pos.PosZ);
        }

        private Position3D GetRot(MapCreatorPosition pos)
        {
            return new Position3D(pos.RotX, pos.RotY, pos.RotZ);
        }

        private void OnMapClearMethod(object[] args)
        {
            try
            {
                foreach (var obj in _objects)
                {
                    obj.Destroy();
                }
                _objects.Clear();

                _eventsHandler.OnMapCleared();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnMapChangeMethod(object[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var mapData = _serializer.FromServer<ClientSyncedDataDto>((string)args[0]);
                    SetMapData(mapData);
                }
            
                ModAPI.Graphics.StopScreenEffect(EffectName.DEATHFAILMPIN);
                ModAPI.Cam.SetCamEffect(0);
                ModAPI.Cam.DoScreenFadeIn(_settingsHandler.MapChooseTime);

                _eventsHandler.OnMapChanged();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
}
    }
}
