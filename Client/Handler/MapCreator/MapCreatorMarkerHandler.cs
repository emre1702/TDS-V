using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw.Dx;
using TDS.Shared.Data.Enums;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorMarkerHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly ClickedMarkerStorer _clickedMarkerStorer;
        private readonly DxHandler _dxHandler;
        private readonly MapCreatorDrawHandler _mapCreatorDrawHandler;
        private readonly MapCreatorObjectPlacingHandler _mapCreatorObjectPlacingHandler;
        private readonly MapCreatorSyncHandler _mapCreatorSyncHandler;
        private readonly UtilsHandler _utilsHandler;
        private AxisMarker _highlightedMarker;
        private AxisMarker[] _rotateMarker;

        public MapCreatorMarkerHandler(LoggingHandler loggingHandler, UtilsHandler utilsHandler, DxHandler dxHandler,
            CamerasHandler camerasHandler, BrowserHandler browserHandler,
            MapCreatorDrawHandler mapCreatorDrawHandler, MapCreatorObjectPlacingHandler mapCreatorObjectPlacingHandler,
            MapCreatorSyncHandler mapCreatorSyncHandler, ClickedMarkerStorer clickedMarkerStorer)
            : base(loggingHandler)
        {
            _utilsHandler = utilsHandler;
            _dxHandler = dxHandler;
            _camerasHandler = camerasHandler;
            _browserHandler = browserHandler;
            _mapCreatorDrawHandler = mapCreatorDrawHandler;
            _mapCreatorObjectPlacingHandler = mapCreatorObjectPlacingHandler;
            _mapCreatorSyncHandler = mapCreatorSyncHandler;
            _clickedMarkerStorer = clickedMarkerStorer;
        }

        public void OnTick()
        {
            if (_rotateMarker == null)
                return;

            if (_mapCreatorObjectPlacingHandler.LastHighlightedObject == null || _mapCreatorObjectPlacingHandler.HoldingObject != null)
                return;

            var obj = _mapCreatorObjectPlacingHandler.LastHighlightedObject;
            float rotateScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) * 1.25f;
            float moveScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) / 15f;

            if (_clickedMarkerStorer.ClickedMarker != null)
            {
                if (RAGE.Game.Pad.IsDisabledControlJustReleased((int)InputGroup.MOVE, (int)Control.Attack))
                {
                    _clickedMarkerStorer.ClickedMarker = null;
                    obj.Position = obj.MovingPosition.Copy();
                    obj.Rotation = obj.MovingRotation.Copy();
                    _browserHandler.Angular.AddPositionToMapCreatorBrowser(obj.Id, obj.Type, obj.Position.X, obj.Position.Y, obj.Position.Z,
                        obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.TeamNumber.HasValue ? obj.TeamNumber.Value : (object)obj.ObjOrVehName, obj.OwnerRemoteId);
                    _mapCreatorSyncHandler.SyncObjectPositionToLobby(obj);
                }
                else
                {
                    _clickedMarkerStorer.ClickedMarker.LoadObjectData(obj, _clickedMarkerStorer.ClickedMarker.IsRotationMarker ? rotateScale : moveScale);
                    _clickedMarkerStorer.ClickedMarker.Draw();
                    _clickedMarkerStorer.ClickedMarker.MoveOrRotateObject(obj);
                }
            }
            else
            {
                float closestDistToMarker = float.MaxValue;
                AxisMarker closestMarker = null;
                Vector3 hitPointClosestMarker = null;
                IEnumerable<AxisMarker> markerList;
                switch (obj.Type)
                {
                    case MapCreatorPositionType.Target:
                    case MapCreatorPositionType.MapCenter:
                    case MapCreatorPositionType.MapLimit:
                        markerList = _rotateMarker.Where(m => m.IsPositionMarker);
                        break;

                    case MapCreatorPositionType.TeamSpawn:
                        markerList = _rotateMarker.Where(m => m.IsPositionMarker || m.Axis == AxisMarker.AxisEnum.Z);
                        break;

                    default:
                        markerList = _rotateMarker;
                        break;
                }

                foreach (var marker in markerList)
                {
                    marker.LoadObjectData(obj, marker.IsRotationMarker ? rotateScale : moveScale);
                    marker.CheckClosest(ref closestDistToMarker, ref closestMarker, ref hitPointClosestMarker);
                }

                if (_highlightedMarker != closestMarker)
                {
                    _highlightedMarker?.SetNotHighlighted();
                    _highlightedMarker = closestMarker;
                    _highlightedMarker?.SetHighlighted(hitPointClosestMarker);
                }

                if (_highlightedMarker != null && _highlightedMarker.IsPositionMarker)
                    // false comes first, so if we want to have RotationMarker first, we have to use
                    // ! before it
                    foreach (var marker in markerList.OrderBy(m => !m.IsRotationMarker))
                        marker.Draw();
                else
                    // false comes first, so if we want to have PositionMarker first, we have to use
                    // ! before it
                    foreach (var marker in markerList.OrderBy(m => !m.IsPositionMarker))
                        marker.Draw();
            }

            if (RAGE.Game.Pad.IsDisabledControlJustPressed((int)InputGroup.MOVE, (int)Control.Attack))
            {
                if (_highlightedMarker is null)
                {
                    _mapCreatorObjectPlacingHandler.LastHighlightedObject = null;
                }
                else
                {
                    _clickedMarkerStorer.ClickedMarker = _highlightedMarker;
                    _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(255, 255, 255, 0);
                    _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 0);
                }
            }
            else if (_mapCreatorObjectPlacingHandler.HighlightedObject == null)
            {
                _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(255, 255, 255, 255);
                _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 255);
            }
        }

        public void Start()
        {
            float mapMoveZGetter(MapCreatorObject obj) => obj.Type == MapCreatorPositionType.TeamSpawn ? obj.Size.Z / 2 + 0.2f : obj.Size.Z + 0.2f;

            _rotateMarker = new AxisMarker[]
            {
                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.HorizontalSplitArrowCircle, Color.FromArgb(255, 0, 0), AxisMarker.AxisEnum.X,
                    rotationGetter: obj => new Vector3(-obj.MovingRotation.Z + 90f, 90, 0),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X, obj.Rotation.Y + angle, obj.Rotation.Z)),     // Marker_X_Rotate

                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.HorizontalSplitArrowCircle, Color.FromArgb(0, 255, 0), AxisMarker.AxisEnum.Y,
                    rotationGetter: obj => new Vector3(-obj.MovingRotation.Z, 90, 0),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X - angle, obj.Rotation.Y, obj.Rotation.Z)),     // Marker_Y_Rotate

                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.HorizontalSplitArrowCircle, Color.FromArgb(0, 0, 255), AxisMarker.AxisEnum.Z,
                    rotationGetter: obj => new Vector3(),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z - angle)),     // Marker_Z_Rotate

                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.DebugSphere, Color.FromArgb(255, 0, 0), AxisMarker.AxisEnum.X,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(obj.Size.X / 2f + 0.2f, 0f, 0f),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(-1000f, 0f, 0f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(1000f, 0f, 0f)),    // Marker_X_Move

                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.DebugSphere, Color.FromArgb(0, 255, 0), AxisMarker.AxisEnum.Y,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, obj.Size.Y / 2f + 0.2f, 0f),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(0, -1000f, 0f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 1000f, 0f)),    // Marker_Y_Move

                new AxisMarker(_utilsHandler, _dxHandler, _camerasHandler, MarkerType.DebugSphere, Color.FromArgb(0, 0, 255), AxisMarker.AxisEnum.Z,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, mapMoveZGetter(obj)),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, -1000f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, 1000f)),    // Marker_Z_Move
            };
            _clickedMarkerStorer.ClickedMarker = null;
        }

        public void Stop()
        {
            _rotateMarker = null;
            _clickedMarkerStorer.ClickedMarker = null;
        }
    }
}