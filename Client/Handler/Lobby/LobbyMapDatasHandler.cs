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
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyMapDatasHandler
    {
        public ClientSyncedDataDto MapDatas { get; set; }

        private DxText _mapInfo;
        private readonly List<IEntity> _objects = new List<IEntity>();

        private readonly IModAPI _modAPI;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly LobbyCamHandler _lobbyCamHandler;
        private readonly MapLimitHandler _mapLimitHandler;

        public LobbyMapDatasHandler(IModAPI modAPI, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler, LobbyCamHandler lobbyCamHandler, MapLimitHandler mapLimitHandler)
        {
            _modAPI = modAPI;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _lobbyCamHandler = lobbyCamHandler;
            _mapLimitHandler = mapLimitHandler;

            eventsHandler.MapCleared += CustomEventManager_OnMapClear;
            eventsHandler.LobbyLeft += CustomEventManager_OnLobbyLeave;
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

        private void CustomEventManager_OnLobbyLeave(SyncedLobbySettingsDto settings)
        {
            CustomEventManager_OnMapClear();
            MapDatas = null;
            RemoveMapInfo();
        }

        private void CustomEventManager_OnMapClear()
        {
            foreach (var obj in _objects)
            {
                obj.Destroy();
            }
            _objects.Clear();
        }

        public void RemoveMapInfo()
        {
            _mapInfo?.Remove();
            _mapInfo = null;
        }

        private void LoadMap(ClientSyncedDataDto map)
        {
            CustomEventManager_OnMapClear();

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
                    var veh = _modAPI.Vehicle.Create(vehHash, GetPos(data), GetRot(data), map.Name, true, _modAPI.LocalPlayer.Dimension);
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
    }
}
