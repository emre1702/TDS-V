using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Extensions;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities.GTA;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Map;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Default;
using static RAGE.NUI.UIResText;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyMapDatasHandler : ServiceBase
    {
        private readonly DxHandler _dxHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbyCamHandler _lobbyCamHandler;
        private readonly MapLimitHandler _mapLimitHandler;
        private readonly List<GameEntityBase> _objects = new List<GameEntityBase>();

        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private DxText _mapInfo;

        public LobbyMapDatasHandler(LoggingHandler loggingHandler, DxHandler dxHandler, TimerHandler timerHandler, EventsHandler eventsHandler,
            LobbyCamHandler lobbyCamHandler, MapLimitHandler mapLimitHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _lobbyCamHandler = lobbyCamHandler;
            _mapLimitHandler = mapLimitHandler;

            _settingsHandler = settingsHandler;
            _eventsHandler = eventsHandler;

            eventsHandler.LobbyLeft += CustomEventManager_OnLobbyLeave;

            RAGE.Events.Add(ToClientEvent.MapChange, OnMapChangeMethod);
            RAGE.Events.Add(ToClientEvent.MapClear, OnMapClearMethod);
        }

        public ClientSyncedDataDto MapDatas { get; set; }

        public void RemoveMapInfo()
        {
            _mapInfo?.Remove();
            _mapInfo = null;
        }

        public void SetMapData(ClientSyncedDataDto mapData)
        {
            try
            {
                Logging.LogInfo("", "LobbyMapDatasHandler.SetMapData");
                MapDatas = mapData;

                if (_mapInfo == null)
                    _mapInfo = new DxText(_dxHandler, _timerHandler, MapDatas.Name, 0.01f, 0.995f, 0.2f, Color.White, Alignment: Alignment.Left, alignmentY: AlignmentY.Bottom);
                else
                    _mapInfo.Text = MapDatas.Name;

                LoadMap(mapData);

                if (mapData.Target != null)
                    _lobbyCamHandler.SetToMapCenter(mapData.Target.ToVector3());
                else if (mapData.Center != null)
                    _lobbyCamHandler.SetToMapCenter(mapData.Center.ToVector3());

                if (mapData.MapEdges != null && mapData.MapEdges.Count > 0)
                    _mapLimitHandler.Load(mapData.MapEdges.Select(e => e.ToVector3()).ToList());
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

        private Vector3 GetPos(MapCreatorPosition pos)
        {
            return new Vector3(pos.PosX, pos.PosY, pos.PosZ);
        }

        private Vector3 GetRot(MapCreatorPosition pos)
        {
            return new Vector3(pos.RotX, pos.RotY, pos.RotZ);
        }

        private void LoadMap(ClientSyncedDataDto map)
        {
            try
            {
                OnMapClearMethod(Array.Empty<object>());

                if (map.Target != null)
                {
                    var obj = new TDSObject(RAGE.Game.Misc.GetHashKey(Constants.TargetHashName), map.Target.ToVector3(), new RAGE.Vector3(), dimension: Player.LocalPlayer.Dimension);
                    obj.FreezePosition(true);
                    //obj.SetCollision(false, true);
                    obj.SetInvincible(true);
                    _objects.Add(obj);
                }

                if (map.Objects != null)
                {
                    foreach (var data in map.Objects)
                    {
                        string objName = Convert.ToString(data.Info);
                        uint objectHash = RAGE.Game.Misc.GetHashKey(objName);
                        var obj = new TDSObject(objectHash, GetPos(data), GetRot(data), dimension: Player.LocalPlayer.Dimension);
                        obj.FreezePosition(true);
                        obj.SetInvincible(true);
                        _objects.Add(obj);
                    }
                }

                if (map.BombPlaces != null)
                {
                    foreach (var data in map.BombPlaces)
                    {
                        var bombPlantHash = RAGE.Game.Misc.GetHashKey(Constants.BombPlantPlaceHashName);
                        var obj = new TDSObject(bombPlantHash, data.ToVector3(), new Vector3(), dimension: Player.LocalPlayer.Dimension);
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
                        uint vehHash = RAGE.Game.Misc.GetHashKey(vehName);
                        var veh = new TDSVehicle(vehHash, GetPos(data), data.RotZ, map.Name, locked: true, dimension: Player.LocalPlayer.Dimension);
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

        private void OnMapChangeMethod(object[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var mapData = Serializer.FromServer<ClientSyncedDataDto>((string)args[0]);
                    SetMapData(mapData);
                }

                RAGE.Game.Graphics.StopScreenEffect(EffectName.DEATHFAILMPIN);
                RAGE.Game.Cam.SetCamEffect(0);
                RAGE.Game.Cam.DoScreenFadeIn(_settingsHandler.MapChooseTime);

                _eventsHandler.OnMapChanged();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
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
    }
}