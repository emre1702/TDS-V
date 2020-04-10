﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw.Dx;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorMarkerHandler
    {
        private AxisMarker[] _rotateMarker;
        public AxisMarker ClickedMarker;
        private AxisMarker _highlightedMarker;

        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;
        private readonly DxHandler _dxHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly MapCreatorDrawHandler _mapCreatorDrawHandler;

        public MapCreatorMarkerHandler(IModAPI modAPI, UtilsHandler utilsHandler, DxHandler dxHandler, CamerasHandler camerasHandler, BrowserHandler browserHandler,
            MapCreatorDrawHandler mapCreatorDrawHandler)
        {
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;
            _dxHandler = dxHandler;
            _camerasHandler = camerasHandler;
            _browserHandler = browserHandler;
            _mapCreatorDrawHandler = mapCreatorDrawHandler;
        }

        public void Start()
        {
            _rotateMarker = new AxisMarker[]
            {
                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 27, Color.FromArgb(255, 0, 0), AxisMarker.AxisEnum.X,
                    rotationGetter: obj => new Position3D(-obj.MovingRotation.Z + 90f, 90, 0),
                    objectRotationGetter: (obj, angle) => new Position3D(obj.Rotation.X, obj.Rotation.Y + angle, obj.Rotation.Z)),     // Marker_X_Rotate

                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 27, Color.FromArgb(0, 255, 0), AxisMarker.AxisEnum.Y,
                    rotationGetter: obj => new Position3D(-obj.MovingRotation.Z, 90, 0),
                    objectRotationGetter: (obj, angle) => new Position3D(obj.Rotation.X - angle, obj.Rotation.Y, obj.Rotation.Z)),     // Marker_Y_Rotate

                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 27, Color.FromArgb(0, 0, 255), AxisMarker.AxisEnum.Z,
                    rotationGetter: obj => new Position3D(),
                    objectRotationGetter: (obj, angle) => new Position3D(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z - angle)),     // Marker_Z_Rotate

                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 28, Color.FromArgb(255, 0, 0), AxisMarker.AxisEnum.X,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(obj.Size.X / 2f + (obj.Size.X / 4f), 0f, 0f),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(-1000f, 0f, 0f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(1000f, 0f, 0f)),    // Marker_X_Move

                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 28, Color.FromArgb(0, 255, 0),AxisMarker.AxisEnum.Y,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, obj.Size.Y / 2f + (obj.Size.Y / 4f), 0f),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(0, -1000f, 0f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 1000f, 0f)),    // Marker_Y_Move

                new AxisMarker(_modAPI, _utilsHandler, _dxHandler, _camerasHandler, 28, Color.FromArgb(0, 0, 255),AxisMarker.AxisEnum.Z,
                    positionGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, obj.Size.Z + (obj.Size.Z / 4f)),
                    objectPositionFromGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, -1000f),
                    objectPositionToGetter: obj => obj.Entity.GetOffsetInWorldCoords(0f, 0f, 1000f)),    // Marker_Z_Move
            };
            ClickedMarker = null;
        }

        public void Stop()
        {
            _rotateMarker = null;
            ClickedMarker = null;
        }

        public void OnTick()
        {
            if (_rotateMarker == null)
                return;

            if (MapCreatorObjectPlacingHandler.LastHighlightedObject == null || MapCreatorObjectPlacingHandler.HoldingObject != null)
                return;

            var obj = MapCreatorObjectPlacingHandler.LastHighlightedObject;
            float rotateScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) * 1.25f;
            float moveScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) / 15f;

            if (ClickedMarker != null)
            {
                if (_modAPI.Control.IsDisabledControlJustReleased(InputGroup.MOVE, Control.Attack))
                {
                    ClickedMarker = null;
                    _browserHandler.Angular.AddPositionToMapCreatorBrowser(obj.ID, obj.Type, obj.MovingPosition.X, obj.MovingPosition.Y, obj.MovingPosition.Z,
                        obj.MovingRotation.X, obj.MovingRotation.Y, obj.MovingRotation.Z, obj.ObjOrVehName, obj.OwnerRemoteId);
                }
                else
                {
                    ClickedMarker.LoadObjectData(obj, ClickedMarker.IsRotationMarker ? rotateScale : moveScale);
                    ClickedMarker.Draw();
                    ClickedMarker.MoveOrRotateObject(obj);
                }
            }
            else
            {

                float closestDistToMarker = float.MaxValue;
                AxisMarker closestMarker = null;
                Position3D hitPointClosestMarker = null;
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
                    // false comes first, so if we want to have RotationMarker first, we have to use ! before it
                    foreach (var marker in markerList.OrderBy(m => !m.IsRotationMarker))
                        marker.Draw();
                else
                    // false comes first, so if we want to have PositionMarker first, we have to use ! before it
                    foreach (var marker in markerList.OrderBy(m => !m.IsPositionMarker))
                        marker.Draw();
            }

            if (_modAPI.Control.IsDisabledControlJustPressed(InputGroup.MOVE, Control.Attack))
            {
                ClickedMarker = _highlightedMarker;
                _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(255, 255, 255, 0);
                _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 0);
            }
            else if (MapCreatorObjectPlacingHandler.HighlightedObject == null)
            {
                _mapCreatorDrawHandler.HighlightColor_Edge = Color.FromArgb(255, 255, 255, 255);
                _mapCreatorDrawHandler.HighlightColor_Full = Color.FromArgb(35, 255, 255, 255);
            }
        }

    }
}
