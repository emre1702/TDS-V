using System;
using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class AxisMarker
    {
        public enum AxisEnum
        {
            X, Y, Z
        }

        public readonly Marker Marker;

        public Position3D HighlightStartHitPoint;

        public bool IsRotationMarker => _rotationGetter != null;
        public bool IsPositionMarker => _positionGetter != null;
        public AxisEnum Axis;

        private readonly Func<MapCreatorObject, Position3D> _positionGetter;
        private readonly Func<MapCreatorObject, Position3D> _objectPositionFromGetter;
        private readonly Func<MapCreatorObject, Position3D> _objectPositionToGetter;
        private readonly Func<MapCreatorObject, Position3D> _rotationGetter;
        private readonly Func<MapCreatorObject, float, Position3D> _objectRotationGetter;

        private readonly Color _originalColor;

        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;
        private readonly DxHandler _dxHandler;
        private readonly CamerasHandler _camerasHandler;

        public AxisMarker(
            IModAPI modAPI,
            UtilsHandler utilsHandler,
            DxHandler dxHandler,
            CamerasHandler camerasHandler,
            int type,
            Color color,
            AxisEnum axis,
            Func<MapCreatorObject, Position3D> positionGetter = null,
            Func<MapCreatorObject, Position3D> objectPositionFromGetter = null,
            Func<MapCreatorObject, Position3D> objectPositionToGetter = null,
            Func<MapCreatorObject, Position3D> rotationGetter = null,
            Func<MapCreatorObject, float, Position3D> objectRotationGetter = null)
        {
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;
            _dxHandler = dxHandler;
            _camerasHandler = camerasHandler;

            _originalColor = color;
            Marker = new Marker(modAPI, type, new Position3D(), new Position3D(), new Position3D(), new Position3D(), color);
            Axis = axis;
            _positionGetter = positionGetter;
            _objectPositionFromGetter = objectPositionFromGetter;
            _objectPositionToGetter = objectPositionToGetter;
            _rotationGetter = rotationGetter;
            _objectRotationGetter = objectRotationGetter;
        }

        public void SetHighlighted(Position3D highlightStartHitPoint)
        {
            HighlightStartHitPoint = highlightStartHitPoint;
            Marker.Color = Color.FromArgb(255, 255, 0);
        }

        public void SetNotHighlighted()
        {
            Marker.Color = _originalColor;
        }

        public bool IsRaycasted(ref Position3D hitPoint, ref Position3D norm, TDSCamera cam = null, float threshold = 0.1f, bool ignoreDistance = false)
        {
            Position3D test1 = cam?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
            Position3D test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            Position3D test3 = test2 - test1;
            Position3D from = test1 + test3 * 0.05f;
            Position3D to = test1 + test3 * 1000f;

            if (!ignoreDistance)
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Position3D(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), Marker.Scale.X / 2f, from, to, ref hitPoint, threshold, ref norm))
                    if (Marker.Position.DistanceTo(hitPoint) >= (Marker.Scale.X / 2f) * 0.775f - threshold)
                        return true;
            }
            else
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Position3D(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), threshold, from, to, ref hitPoint, 0f, ref norm))
                    return true;
            }
            return false;
        }

        public bool IsSphereCasted(TDSCamera cam = null)
        {
            Position3D test1 = cam?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
            Position3D test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            Position3D test3 = test2 - test1;
            Position3D from = test1 + test3 * 0.05f;
            Position3D to = test1 + test3 * 1000f;

            return _utilsHandler.LineIntersectingSphere(from, to, Marker.Position, Marker.Scale.X);
        }

        public void Draw()
        {
            Marker.Draw();
            Position3D v = _utilsHandler.GetScreenCoordFromWorldCoord(Marker.Position);
            if (v != null)
            {
                var camPos = _camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
                float dist = Marker.Position.DistanceTo(camPos);
                if (IsPositionMarker)
                    _modAPI.Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * (_dxHandler.ResY / _dxHandler.ResX), Marker.Scale.X * 4 / dist, 0, 
                        Marker.Color.R, Marker.Color.G, Marker.Color.B, Marker.Color.A, 0);
            }
        }

        public void MoveOrRotateObject(MapCreatorObject obj)
        {
            if (IsRotationMarker)
            {
                Position3D hitPoint = new Position3D();
                Position3D norm = new Position3D();
                if (!IsRaycasted(ref hitPoint, ref norm, _camerasHandler.ActiveCamera, 999999f, true))
                    return;

                float angle = _utilsHandler.RadToDegrees(_utilsHandler.GetAngleBetweenVectors(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position));
                float sign = MathF.Sign(_utilsHandler.GetDotProduct(norm, _utilsHandler.GetCrossProduct(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position)));
                angle *= sign;
                obj.MovingRotation = _objectRotationGetter(obj, angle);
            }
            else
            {
                Position3D test1 = _camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
                Position3D test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), _camerasHandler.ActiveCamera);
                Position3D test3 = test2 - test1;
                Position3D from = test1 + test3 * 0.05f;
                Position3D to = test1 + test3 * 1000f;

                Position3D from1 = _objectPositionFromGetter(obj);
                Position3D to1 = _objectPositionToGetter(obj);
                _modAPI.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                _modAPI.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                Tuple<Position3D, Position3D> drawMe = _utilsHandler.ClosestDistanceBetweenLines(from, to, from1, to1);
                obj.MovingPosition = drawMe.Item2 - (Marker.Position - obj.MovingPosition);
            }
        }

        public void LoadObjectData(MapCreatorObject obj, float scale)
        {
            if (_positionGetter != null)
                Marker.Position = _positionGetter(obj);
            else
                Marker.Position = obj.MovingPosition;

            if (_rotationGetter != null)
                Marker.Rotation = _rotationGetter(obj);

            Marker.Scale = new Position3D(scale, scale, scale);
        }

        public void CheckClosest(ref float closestDist, ref AxisMarker closestMarker, ref Position3D hitPointClosestMarker)
        {
            if (IsRotationMarker)
                CheckClosestRotateMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
            else
                CheckClosestMoveMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
        }

        private void CheckClosestRotateMarker(ref float closestDist, ref AxisMarker closestMarker, ref Position3D hitPointClosestMarker)
        {
            Position3D hitPoint = new Position3D();
            Position3D norm = new Position3D();
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

        private void CheckClosestMoveMarker(ref float closestDist, ref AxisMarker closestMarker, ref Position3D hitPointClosestMarker)
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
    }
}
