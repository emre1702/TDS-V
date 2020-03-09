using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Shared.Dto;
using TDS_Shared.Dto.Map;
using TDS_Shared.Dto.Map.Creator;
using Entity = RAGE.Elements.Entity;
using Player = RAGE.Elements.Player;
using Vehicle = RAGE.Elements.Vehicle;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapDataManager
    {
        public static ClientSyncedDataDto MapData { get; set; }

        private static DxText _mapInfo;
        private static bool _eventBinded;
        private static List<Entity> _objects = new List<Entity>();

        public static void SetMapData(ClientSyncedDataDto mapData)
        {
            MapData = mapData;

            if (_mapInfo == null)
                _mapInfo = new DxText(MapData.Name, 0.01f, 0.995f, 0.2f, Color.White, alignmentX: RAGE.NUI.UIResText.Alignment.Left, alignmentY: EAlignmentY.Bottom);
            else
                _mapInfo.Text = MapData.Name;

            if (!_eventBinded)
            {
                _eventBinded = true;
                CustomEventManager.OnMapClear += CustomEventManager_OnMapClear;
                CustomEventManager.OnLobbyLeave += CustomEventManager_OnLobbyLeave;
            }

            LoadMap(mapData);

            if (mapData.Target != null)
                LobbyCam.SetToMapCenter(mapData.Target);
            else if (mapData.Center != null)
                LobbyCam.SetToMapCenter(mapData.Center);

            if (mapData.MapEdges != null && mapData.MapEdges.Count > 0)
                MapLimitManager.Load(mapData.MapEdges);
        }

        private static void CustomEventManager_OnLobbyLeave(SyncedLobbySettingsDto settings)
        {
            CustomEventManager_OnMapClear();
            MapData = null;
        }

        private static void CustomEventManager_OnMapClear()
        {
            foreach (var obj in _objects)
            {
                obj.Destroy();
            }
            _objects.Clear();
        }

        public static void RemoveMapInfo()
        {
            _mapInfo?.Remove();
            _mapInfo = null;
        }

        private static void LoadMap(ClientSyncedDataDto map)
        {
            CustomEventManager_OnMapClear();

            if (map.Target != null)
            {
                var obj = new MapObject(ClientConstants.TargetHash, map.Target.ToVector3(), new Vector3(), dimension: Player.LocalPlayer.Dimension);
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
                    uint objectHash = Misc.GetHashKey(objName);
                    var obj = new MapObject(objectHash, data.GetPos(), data.GetRot(), dimension: Player.LocalPlayer.Dimension);
                    obj.FreezePosition(true);
                    obj.SetInvincible(true);
                    _objects.Add(obj);
                }
            }

            if (map.BombPlaces != null)
            {
                foreach (var data in map.BombPlaces)
                {
                    var obj = new MapObject(ClientConstants.BombPlantPlaceHash, data.ToVector3(), new Vector3(), dimension: Player.LocalPlayer.Dimension);
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
                    uint vehHash = Misc.GetHashKey(vehName);
                    var veh = new Vehicle(vehHash, data.GetPos(), data.RotZ, map.Name, 255, true, dimension: Player.LocalPlayer.Dimension);
                    veh.FreezePosition(true);
                    veh.SetInvincible(true);
                    _objects.Add(veh);
                }
            }
        }

        private static Vector3 ToVector3(this Position3DDto pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        private static Vector3 GetPos(this MapCreatorPosition pos)
        {
            return new Vector3(pos.PosX, pos.PosY, pos.PosZ);
        }

        private static Vector3 GetRot(this MapCreatorPosition pos)
        {
            return new Vector3(pos.RotX, pos.RotY, pos.RotZ);
        }
    }
}
