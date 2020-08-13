using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class AxisMarker
    {
        #region Public Fields

        public readonly Marker Marker;

        public AxisEnum Axis;

        public Position HighlightStartHitPoint;

        #endregion Public Fields

        #region Private Fields

        private readonly CamerasHandler _camerasHandler;

        private readonly DxHandler _dxHandler;

        private readonly IModAPI _modAPI;

        private readonly Func<MapCreatorObject, Position> _objectPositionFromGetter;

        private readonly Func<MapCreatorObject, Position> _objectPositionToGetter;

        private readonly Func<MapCreatorObject, float, Position> _objectRotationGetter;

        private readonly Color _originalColor;

        private readonly Func<MapCreatorObject, Position> _positionGetter;

        private readonly Func<MapCreatorObject, Position> _rotationGetter;

        private readonly UtilsHandler _utilsHandler;

        #endregion Private Fields

        #region Public Constructors

        public AxisMarker(
            IModAPI modAPI,
            UtilsHandler utilsHandler,
            DxHandler dxHandler,
            CamerasHandler camerasHandler,
            MarkerType type,
            Color color,
            AxisEnum axis,
            Func<MapCreatorObject, Position> positionGetter = null,
            Func<MapCreatorObject, Position> objectPositionFromGetter = null,
            Func<MapCreatorObject, Position> objectPositionToGetter = null,
            Func<MapCreatorObject, Position> rotationGetter = null,
            Func<MapCreatorObject, float, Position> objectRotationGetter = null)
        {
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;
            _dxHandler = dxHandler;
            _camerasHandler = camerasHandler;

            _originalColor = color;
            Marker = new Marker(modAPI, type, new Position(), new Position(), new Position(), new Position(), color);
            Axis = axis;
            _positionGetter = positionGetter;
            _objectPositionFromGetter = objectPositionFromGetter;
            _objectPositionToGetter = objectPositionToGetter;
            _rotationGetter = rotationGetter;
            _objectRotationGetter = objectRotationGetter;
        }

        #endregion Public Constructors

        #region Public Enums

        public enum AxisEnum
        {
            X, Y, Z
        }

        #endregion Public Enums

        #region Public Properties

        public bool IsPositionMarker => _positionGetter != null;
        public bool IsRotationMarker => _rotationGetter != null;

        #endregion Public Properties

        #region Public Methods

        public void CheckClosest(ref float closestDist, ref AxisMarker closestMarker, ref Position hitPointClosestMarker)
        {
            if (IsRotationMarker)
                CheckClosestRotateMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
            else
                CheckClosestMoveMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
        }

        public void Draw()
        {
            Marker.Draw();
            Position v = _utilsHandler.GetScreenCoordFromWorldCoord(Marker.Position);
            if (v != null)
            {
                var camPos = _camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
                float dist = Marker.Position.DistanceTo(camPos);
                if (IsPositionMarker)
                    _modAPI.Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * (_dxHandler.ResY / _dxHandler.ResX), Marker.Scale.X * 4 / dist, 0,
                        Marker.Color.R, Marker.Color.G, Marker.Color.B, Marker.Color.A);
            }
        }

        public bool IsRaycasted(ref Position hitPoint, ref Position norm, TDSCamera cam = null, float threshold = 0.1f, bool ignoreDistance = false)
        {
            Position test1 = cam?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
            Position test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            Position test3 = test2 - test1;
            Position from = test1 + test3 * 0.05f;
            Position to = test1 + test3 * 1000f;

            if (!ignoreDistance)
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Position(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), Marker.Scale.X / 2f, from, to, ref hitPoint, threshold, ref norm))
                    if (Marker.Position.DistanceTo(hitPoint) >= (Marker.Scale.X / 2f) * 0.775f - threshold)
                        return true;
            }
            else
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Position(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), threshold, from, to, ref hitPoint, 0f, ref norm))
                    return true;
            }
            return false;
        }

        public bool IsSphereCasted(TDSCamera cam = null)
        {
            Position test1 = cam?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
            Position test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            Position test3 = test2 - test1;
            Position from = test1 + test3 * 0.05f;
            Position to = test1 + test3 * 1000f;

            return _utilsHandler.LineIntersectingSphere(from, to, Marker.Position, Marker.Scale.X);
        }

        public void LoadObjectData(MapCreatorObject obj, float scale)
        {
            if (_positionGetter != null)
                Marker.Position = _positionGetter(obj);
            else
                Marker.Position = obj.MovingPosition;

            if (_rotationGetter != null)
                Marker.Rotation = _rotationGetter(obj);

            Marker.Scale = new Position(scale, scale, scale);
        }

        public void MoveOrRotateObject(MapCreatorObject obj)
        {
            if (IsRotationMarker)
            {
                Position hitPoint = new Position();
                Position norm = new Position();
                if (!IsRaycasted(ref hitPoint, ref norm, _camerasHandler.ActiveCamera, 999999f, true))
                    return;

                float angle = _utilsHandler.RadToDegrees(_utilsHandler.GetAngleBetweenVectors(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position));
                float sign = MathF.Sign(_utilsHandler.GetDotProduct(norm, _utilsHandler.GetCrossProduct(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position)));
                angle *= sign;
                obj.MovingRotation = _objectRotationGetter(obj, angle);
            }
            else
            {
                Position test1 = _camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
                Position test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), _camerasHandler.ActiveCamera);
                Position test3 = test2 - test1;
                Position from = test1 + test3 * 0.05f;
                Position to = test1 + test3 * 1000f;

                Position from1 = _objectPositionFromGetter(obj);
                Position to1 = _objectPositionToGetter(obj);
                _modAPI.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                _modAPI.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                Tuple<Position, Position> drawMe = _utilsHandler.ClosestDistanceBetweenLines(from, to, from1, to1);
                obj.MovingPosition = drawMe.Item2 - (Marker.Position - obj.MovingPosition);
            }
        }

        public void SetHighlighted(Position highlightStartHitPoint)
        {
            HighlightStartHitPoint = highlightStartHitPoint;
            Marker.Color = Color.FromArgb(255, 255, 0);
        }

        public void SetNotHighlighted()
        {
            Marker.Color = _originalColor;
        }

        #endregion Public Methods

        #region Private Methods

        private void CheckClosestMoveMarker(ref float closestDist, ref AxisMarker closestMarker, ref Position hitPointClosestMarker)
        {
            if (IsSphereCasted(_camerasHandler.ActiveCamera))
            {
                float dist = Marker.Position.DistanceTo(_camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord());
                if (dist < closestDist || closestMarker?.IsRotationMarker == true)
                {
                    closestDist = dist;
                    closestMarker = this;
                    hitPointClosestMarker = Marker.Position;
                }
            }
        }

        private void CheckClosestRotateMarker(ref float closestDist, ref AxisMarker closestMarker, ref Position hitPointClosestMarker)
        {
            Position hitPoint = new Position();
            Position norm = new Position();
            if (IsRaycasted(ref hitPoint, ref norm, _camerasHandler.ActiveCamera))
            {
                float dist = hitPoint.DistanceTo(_camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestMarker = this;
                    hitPointClosestMarker = hitPoint;
                }
            }
        }

        #endregion Private Methods
    }
}
