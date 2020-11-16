using RAGE;
using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Extensions;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Entities;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Lobby;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorObjectPlacingHandler : ServiceBase
    {
#pragma warning disable IDE1006 // Naming Styles
        private const bool ONLY_HOLD_OWN_OBJECTS = true;
#pragma warning restore IDE1006 // Naming Styles

        public MapCreatorObject LastHighlightedObject;

        public MapCreatorObject HighlightedObject
        {
            get => _highlightedObject;
            set
            {
                _highlightedObject = value;
                if (value != null)
                    LastHighlightedObject = value;
            }
        }

        public MapCreatorObject HoldingObject;

        private MapCreatorObject _highlightedObject;
        private float _clampDistance = 50f;
        private bool _placeOnGround = true;

        private readonly MapCreatorDrawHandler _mapCreatorDrawHandler;
        private readonly MapCreatorObjectsHandler _mapCreatorObjectsHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly CamerasHandler _camerasHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly MapCreatorObjectsPreviewHandler _mapCreatorObjectsPreviewHandler;
        private readonly MapCreatorVehiclesPreviewHandler _mapCreatorVehiclesPreviewHandler;
        private readonly MapCreatorSyncHandler _mapCreatorSyncHandler;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;
        private readonly ClickedMarkerStorer _clickedMarkerStorer;

        public MapCreatorObjectPlacingHandler(LoggingHandler loggingHandler, MapCreatorDrawHandler mapCreatorDrawHandler,
            MapCreatorObjectsHandler mapCreatorObjectsHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, LobbyHandler lobbyHandler,
            SettingsHandler settingsHandler, RemoteEventsSender remoteEventsSender, CamerasHandler camerasHandler, InstructionalButtonHandler instructionalButtonHandler,
            UtilsHandler utilsHandler, MapCreatorObjectsPreviewHandler mapCreatorObjectsPreviewHandler, MapCreatorVehiclesPreviewHandler mapCreatorVehiclesPreviewHandler,
            MapCreatorSyncHandler mapCreatorSyncHandler, EventsHandler eventsHandler, DxHandler dxHandler, TimerHandler timerHandler, ClickedMarkerStorer clickedMarkerStorer)
            : base(loggingHandler)
        {
            _mapCreatorDrawHandler = mapCreatorDrawHandler;
            _mapCreatorObjectsHandler = mapCreatorObjectsHandler;
            _cursorHandler = cursorHandler;
            _browserHandler = browserHandler;
            _lobbyHandler = lobbyHandler;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _camerasHandler = camerasHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _utilsHandler = utilsHandler;
            _mapCreatorObjectsPreviewHandler = mapCreatorObjectsPreviewHandler;
            _mapCreatorVehiclesPreviewHandler = mapCreatorVehiclesPreviewHandler;
            _mapCreatorSyncHandler = mapCreatorSyncHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _clickedMarkerStorer = clickedMarkerStorer;

            eventsHandler.MapCreatorObjectDeleted += CheckObjectDeleted;

            RAGE.Events.Add(FromBrowserEvent.HoldMapCreatorObject, OnHoldMapCreatorObjectMethod);
            RAGE.Events.Add(FromBrowserEvent.MapCreatorHighlightPos, args => HighlightObjectWithId((int)args[0]));
            RAGE.Events.Add(FromBrowserEvent.StartMapCreatorPosPlacing, OnStartMapCreatorPosPlacingMethod);
        }

        public void OnTick()
        {
            if (_clickedMarkerStorer.ClickedMarker != null)
                return;

            if (HoldingObject == null && _cursorHandler.Visible)
                HighlightObject();
            else if (HoldingObject != null)
                MoveHoldingObject();

            if (HighlightedObject != null)
                _mapCreatorDrawHandler.DrawSkeleton(HighlightedObject.Position, HighlightedObject.Size, HighlightedObject.Rotation);
        }

        public void StartNewPlacing(MapCreatorPositionType type, object editingTeamIndexOrObjectName)
        {
            MapCreatorObject obj = _mapCreatorObjectsHandler.CreateMapCreatorObject(type, editingTeamIndexOrObjectName,
                RAGE.Elements.Player.LocalPlayer.RemoteId, RAGE.Elements.Player.LocalPlayer.Position, (RAGE.Elements.Player.LocalPlayer as ITDSPlayer).Rotation);
            if (obj == null)
                return;
            if (type == MapCreatorPositionType.Object)
                _mapCreatorObjectsPreviewHandler.Stop();
            else if (type == MapCreatorPositionType.Vehicle)
                _mapCreatorVehiclesPreviewHandler.Stop();

            if (HoldingObject != null)
                ReleaseObject();
            HoldingObject = null;
            HighlightedObject = obj;
            HoldHighlightingObject();
        }

        public void HighlightObjectWithId(int id)
        {
            if (HoldingObject != null)
                return;

            if (id == -1)
                HighlightObject(null);
            else
            {
                var obj = _mapCreatorObjectsHandler.GetByID(id);
                if (obj == null || obj.Entity.IsNull)
                    return;
                if (!obj.IsMine() && !_lobbyHandler.IsLobbyOwner)
                    return;

                HighlightObject(obj);
            }
        }

        public void HoldObjectWithID(int id)
        {
            var obj = _mapCreatorObjectsHandler.GetByID(id);
            if (obj == null || obj.Entity.IsNull)
                return;
            if (!obj.IsMine() && !_lobbyHandler.IsLobbyOwner)
                return;

            if (HoldingObject != null)
                ReleaseObject();

            HoldingObject = null;
            HighlightedObject = obj;
            HoldHighlightingObject();
        }

        public void LeftMouseClick(Control _)
        {
            if (_clickedMarkerStorer.ClickedMarker != null)
                return;

            if (HoldingObject == null && HighlightedObject != null)
                HoldHighlightingObject();
            else if (HoldingObject != null)
                ReleaseObject();
        }

        public void DeleteHoldingObject(Key _)
        {
            if (HoldingObject == null)
                return;
            _browserHandler.Angular.RemovePositionInMapCreatorBrowser(HoldingObject.ID, HoldingObject.Type);
            var objType = HoldingObject.Type;
            HoldingObject.Delete(true);
            if (HighlightedObject == HoldingObject)
                HighlightedObject = null;
            HoldingObject = null;

            if (objType == MapCreatorPositionType.MapLimit)
            {
                _mapCreatorObjectsHandler.RefreshMapLimitDisplay();
            }
        }

        public void CheckObjectDeleted()
        {
            if (HoldingObject?.Deleted == true)
                HoldingObject = null;
            if (HighlightedObject?.Deleted == true)
                HighlightedObject = null;
            if (LastHighlightedObject?.Deleted == true)
                LastHighlightedObject = null;
        }

        private void HoldHighlightingObject()
        {
            HoldingObject = HighlightedObject;
            HighlightedObject = null;
            HoldingObject.LoadEntityData();

            _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(35, 255, 255, 0);
            _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 0);
        }

        private void ReleaseObject()
        {
            HoldingObject.Position = HoldingObject.MovingPosition.Copy();
            HoldingObject.Rotation = HoldingObject.MovingRotation.Copy();
            var obj = HoldingObject;
            HoldingObject = null;
            object info = null;

            switch (obj.Type)
            {
                case MapCreatorPositionType.TeamSpawn:
                    info = obj.TeamNumber.Value;
                    break;

                case MapCreatorPositionType.Object:
                case MapCreatorPositionType.Vehicle:
                    info = obj.ObjOrVehName;
                    break;
            }
            _browserHandler.Angular.AddPositionToMapCreatorBrowser(obj.ID, obj.Type, obj.Position.X, obj.Position.Y, obj.Position.Z,
                obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, info, obj.OwnerRemoteId);

            _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(35, 255, 255, 255);
            _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 255);

            if (obj.Type == MapCreatorPositionType.MapLimit)
            {
                if (_mapCreatorObjectsHandler.MapLimitDisplay == null)
                {
                    _mapCreatorObjectsHandler.MapLimitDisplay = new MapLimit(new List<Vector3>(), MapLimitType.Display, 0, _settingsHandler.MapBorderColor,
                        _remoteEventsSender, _settingsHandler, _dxHandler, _timerHandler);
                    _mapCreatorObjectsHandler.MapLimitDisplay.Start();
                }
                _mapCreatorObjectsHandler.RefreshMapLimitDisplay();
            }

            if (!obj.IsSynced)
                _mapCreatorSyncHandler.SyncNewObjectToLobby(obj);
            else
                _mapCreatorSyncHandler.SyncObjectPositionToLobby(obj);
        }

        private void MoveHoldingObject()
        {
            if (_cursorHandler.Visible)
                MoveHoldingObjectWithCursor();
            else
                MoveHoldingObjectWithCamera();

            if (_placeOnGround)
                PlaceOnGround(HoldingObject);
            HoldingObject.ActivatePhysics();
        }

        private void MoveHoldingObjectWithCursor()
        {
            if (RAGE.Game.Pad.IsDisabledControlJustPressed((int)InputGroup.MOVE, (int)Control.CursorScrollUp))
            {
                _clampDistance += 5f;
                if (_clampDistance > 500f)
                    _clampDistance = 500f;
            }
            else if (RAGE.Game.Pad.IsDisabledControlJustReleased((int)InputGroup.MOVE, (int)Control.CursorScrollDown))
            {
                _clampDistance -= 5f;
                if (_clampDistance < 5f)
                    _clampDistance = 5;
            }

            var camPos = _camerasHandler.FreeCam.Position;
            var hit = GetCursorHit(1000, HoldingObject.Entity.Handle, -1);

            if (hit.Item1.Hit && hit.Item1.EndCoords.DistanceTo(camPos) <= _clampDistance)
            {
                HoldingObject.MovingPosition = hit.Item1.EndCoords;
            }
            else
            {
                HoldingObject.MovingPosition = hit.Item2;
            }
        }

        private void MoveHoldingObjectWithCamera()
        {
            var hit = GetCameraHit(1000, HoldingObject.Entity.Handle, -1);
            var camPos = _camerasHandler.FreeCam.Position;
            if (hit.Item1.Hit && hit.Item1.EndCoords.DistanceTo(camPos) <= _clampDistance)
            {
                HoldingObject.MovingPosition = hit.Item1.EndCoords;
            }
            else
            {
                HoldingObject.MovingPosition = hit.Item2;
            }
        }

        private void HighlightObject()
        {
            var newHighlightedObject = GetHighlightingObject();
            if (newHighlightedObject != null && !newHighlightedObject.IsMine() && !_lobbyHandler.IsLobbyOwner)
                return;

            HighlightObject(newHighlightedObject);
        }

        private void HighlightObject(MapCreatorObject obj)
        {
            if (obj != HighlightedObject)
            {
                RemoveHightlightObject(HighlightedObject);

                if (obj != null)
                    AddHightlightObject(obj);
            }

            HighlightedObject = obj;
        }

        private void AddHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            obj.LoadEntityData();
            obj.Entity.SetAlpha(180, true);
        }

        private void RemoveHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            obj.Entity.SetAlpha(255, true);
        }

        private MapCreatorObject GetHighlightingObject()
        {
            var hit = GetCursorHit(300, RAGE.Elements.Player.LocalPlayer.Handle, 2 | 8 | 16).Item1;
            if (!hit.Hit)
                return null;
            if (hit.EntityHit == 0)
                return null;

            var obj = ONLY_HOLD_OWN_OBJECTS ? _mapCreatorObjectsHandler.GetByHandle(hit.EntityHit) : _mapCreatorObjectsHandler.GetOrCreateByHandle(hit.EntityHit);
            if (obj == null || obj.Entity.IsNull)
                return null;

            return obj;
        }

        public void TogglePlaceOnGround(Key _)
        {
            if (_browserHandler.InInput)
                return;

            _placeOnGround = !_placeOnGround;
            AddInstructionalButton();
        }

        public void AddInstructionalButton()
        {
            _instructionalButtonHandler.Add(_placeOnGround ? _settingsHandler.Language.LET_IT_FLOAT : _settingsHandler.Language.PUT_ON_GROUND, "F");
        }

        private (RaycastHit, Vector3) GetCursorHit(float toDistance, int ignoreHandle, int flags)
        {
            var camPos = _camerasHandler.FreeCam.Position;
            var cursorPos = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), _camerasHandler.FreeCam);
            var difference = cursorPos - camPos;
            var from = camPos + difference * 0.05f;
            var to = camPos + difference * toDistance;

            var t = to - from;
            t.Normalize();
            t *= _clampDistance;
            var v = camPos + t;

            return (_utilsHandler.RaycastFromTo(from, to, ignoreHandle, flags), v);
        }

        private (RaycastHit, Vector3) GetCameraHit(float toDistance, int ignoreHandle, int flags)
        {
            var camPos = _camerasHandler.FreeCam.Position;
            var lookingAtPos = _utilsHandler.GetWorldCoordFromScreenCoord(0.5f, 0.5f, _camerasHandler.FreeCam);
            var difference = lookingAtPos - camPos;
            var from = camPos + difference * 0.05f;
            var to = camPos + difference * toDistance;

            var t = to - from;
            t.Normalize();
            t *= _clampDistance;
            var v = camPos + t;

            return (_utilsHandler.RaycastFromTo(from, to, ignoreHandle, flags), v);
        }

        private void PlaceOnGround(MapCreatorObject obj)
        {
            switch ((EntityType)obj.Entity.Type)
            {
                case EntityType.Object:
                    RAGE.Game.Object.PlaceObjectOnGroundProperly(obj.Entity.Handle);
                    break;

                case EntityType.Vehicle:
                    RAGE.Game.Vehicle.SetVehicleOnGroundProperly(obj.Entity.Handle, 5);
                    obj.MovingPosition = obj.Entity.Position;
                    break;

                case EntityType.Ped:
                    float heightAboveGround = RAGE.Game.Entity.GetEntityHeightAboveGround(obj.Entity.Handle);
                    obj.MovingPosition = new Vector3(obj.MovingPosition.X, obj.MovingPosition.Y, obj.MovingPosition.Z - heightAboveGround + 1f);
                    break;
            }
        }

        private void OnHoldMapCreatorObjectMethod(object[] args)
        {
            int objID = (int)args[0];
            HoldObjectWithID(objID);
        }

        private void OnStartMapCreatorPosPlacingMethod(object[] args)
        {
            MapCreatorPositionType type = (MapCreatorPositionType)(int)args[0];
            object editingTeamIndexOrObjectName = args[1];
            StartNewPlacing(type, editingTeamIndexOrObjectName);
        }
    }
}
