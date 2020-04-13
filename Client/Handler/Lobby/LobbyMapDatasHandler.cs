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
    public class LobbyMapDatasHandler
    {
        public ClientSyncedDataDto MapDatas { get; set; }

        private DxText _mapInfo;
        private readonly List<IEntityBase> _objects = new List<IEntityBase>();

        private readonly IModAPI _modAPI;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly LobbyCamHandler _lobbyCamHandler;
        private readonly MapLimitHandler _mapLimitHandler;
        private readonly Serializer _serializer;
        private readonly SettingsHandler _settingsHandler;
        private readonly EventsHandler _eventsHandler;

        public LobbyMapDatasHandler(IModAPI modAPI, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler, LobbyCamHandler lobbyCamHandler, 
            MapLimitHandler mapLimitHandler, Serializer serializer, SettingsHandler settingsHandler)
        {
            _modAPI = modAPI;
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
            MapDatas = mapData;

            if (_mapInfo == null)
                _mapInfo = new DxText(_dxHandler, _modAPI, _timerHandler, MapDatas.Name, 0.01f, 0.995f, 0.2f, Color.White, alignmentX: AlignmentX.Left, alignmentY: AlignmentY.Bottom);
            else
                _mapInfo.Text = MapDatas.Name;

            LoadMap(mapData);

            if (mapData.Target != null)
                _lobbyCamHandler.SetToMapCenter(mapData.Target);
            else if (mapData.Center != null)
                _lobbyCamHandler.SetToMapCenter(mapData.Center);

            if (mapData.MapEdges != null && mapData.MapEdges.Count > 0)
                _mapLimitHandler.Load(mapData.MapEdges);
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
            OnMapClearMethod(Array.Empty<object>());

            if (map.Target != null)
            {
                var obj = _modAPI.MapObject.Create(_modAPI.Misc.GetHashKey(Constants.TargetHashName), map.Target, new Position3D(), dimension: _modAPI.LocalPlayer.Dimension);
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
                    uint objectHash = _modAPI.Misc.GetHashKey(objName);
                    var obj = _modAPI.MapObject.Create(objectHash, GetPos(data), GetRot(data), dimension: _modAPI.LocalPlayer.Dimension);
                    obj.FreezePosition(true);
                    obj.SetInvincible(true);
                    _objects.Add(obj);
                }
            }

            if (map.BombPlaces != null)
            {
                foreach (var data in map.BombPlaces)
                {
                    var bombPlantHash = _modAPI.Misc.GetHashKey(Constants.BombPlantPlaceHashName);
                    var obj = _modAPI.MapObject.Create(bombPlantHash, data, new Position3D(), dimension: _modAPI.LocalPlayer.Dimension);
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
                    uint vehHash = _modAPI.Misc.GetHashKey(vehName);
                    var veh = _modAPI.Vehicle.Create(vehHash, GetPos(data), GetRot(data), map.Name, locked: true, dimension: _modAPI.LocalPlayer.Dimension);
                    veh.FreezePosition(true);
                    veh.SetInvincible(true);
                    _objects.Add(veh);
                }
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
            foreach (var obj in _objects)
            {
                obj.Destroy();
            }
            _objects.Clear();

            _eventsHandler.OnMapCleared();
        }

        private void OnMapChangeMethod(object[] args)
        {
            if (args.Length > 0)
            {
                var mapData = _serializer.FromServer<ClientSyncedDataDto>((string)args[0]);
                SetMapData(mapData);
            }
            
            _modAPI.Graphics.StopScreenEffect(EffectName.DEATHFAILMPIN);
            _modAPI.Cam.SetCamEffect(0);
            _modAPI.Cam.DoScreenFadeIn(_settingsHandler.MapChooseTime);

            _eventsHandler.OnMapChanged();
        }
    }
}
