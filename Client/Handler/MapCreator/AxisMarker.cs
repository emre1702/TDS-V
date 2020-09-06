using RAGE;
using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class AxisMarker
    {
        public readonly Marker Marker;

        public AxisEnum Axis;

        public Vector3 HighlightStartHitPoint;

        private readonly CamerasHandler _camerasHandler;

        private readonly DxHandler _dxHandler;

        private readonly Func<MapCreatorObject, Vector3> _objectPositionFromGetter;

        private readonly Func<MapCreatorObject, Vector3> _objectPositionToGetter;

        private readonly Func<MapCreatorObject, float, Vector3> _objectRotationGetter;

        private readonly Color _originalColor;

        private readonly Func<MapCreatorObject, Vector3> _positionGetter;

        private readonly Func<MapCreatorObject, Vector3> _rotationGetter;

        private readonly UtilsHandler _utilsHandler;

        public AxisMarker(
            UtilsHandler utilsHandler,
            DxHandler dxHandler,
            CamerasHandler camerasHandler,
            MarkerType type,
            Color color,
            AxisEnum axis,
            Func<MapCreatorObject, Vector3> positionGetter = null,
            Func<MapCreatorObject, Vector3> objectPositionFromGetter = null,
            Func<MapCreatorObject, Vector3> objectPositionToGetter = null,
            Func<MapCreatorObject, Vector3> rotationGetter = null,
            Func<MapCreatorObject, float, Vector3> objectRotationGetter = null)
        {
            _utilsHandler = utilsHandler;
            _dxHandler = dxHandler;
            _camerasHandler = camerasHandler;

            _originalColor = color;
            Marker = new Marker(type, new Vector3(), new Vector3(), new Vector3(), new Vector3(), color);
            Axis = axis;
            _positionGetter = positionGetter;
            _objectPositionFromGetter = objectPositionFromGetter;
            _objectPositionToGetter = objectPositionToGetter;
            _rotationGetter = rotationGetter;
            _objectRotationGetter = objectRotationGetter;
        }

        public enum AxisEnum
        {
            X, Y, Z
        }

        public bool IsPositionMarker => _positionGetter != null;
        public bool IsRotationMarker => _rotationGetter != null;

        public void CheckClosest(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            if (IsRotationMarker)
                CheckClosestRotateMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
            else
                CheckClosestMoveMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
        }

        public void Draw()
        {
            Marker.Draw();
            var v = _utilsHandler.GetScreenCoordFromWorldCoord(Marker.Position);
            if (v != null)
            {
                var camPos = _camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
                float dist = Marker.Position.DistanceTo(camPos);
                if (IsPositionMarker)
                    RAGE.Game.Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * (_dxHandler.ResY / _dxHandler.ResX), Marker.Scale.X * 4 / dist, 0,
                        Marker.Color.R, Marker.Color.G, Marker.Color.B, Marker.Color.A, 0);
            }
        }

        public bool IsRaycasted(ref Vector3 hitPoint, ref Vector3 norm, TDSCamera cam = null, float threshold = 0.1f, bool ignoreDistance = false)
        {
            var test1 = cam?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            var test3 = test2 - test1;
            var from = test1 + test3 * 0.05f;
            var to = test1 + test3 * 1000f;

            if (!ignoreDistance)
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), Marker.Scale.X / 2f, from, to, ref hitPoint, threshold, ref norm))
                    if (Marker.Position.DistanceTo(hitPoint) >= (Marker.Scale.X / 2f) * 0.775f - threshold)
                        return true;
            }
            else
            {
                if (_utilsHandler.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), threshold, from, to, ref hitPoint, 0f, ref norm))
                    return true;
            }
            return false;
        }

        public bool IsSphereCasted(TDSCamera cam = null)
        {
            var test1 = cam?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), cam);
            var test3 = test2 - test1;
            var from = test1 + test3 * 0.05f;
            var to = test1 + test3 * 1000f;

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

            Marker.Scale = new Vector3(scale, scale, scale);
        }

        public void MoveOrRotateObject(MapCreatorObject obj)
        {
            if (IsRotationMarker)
            {
                var hitPoint = new Vector3();
                var norm = new Vector3();
                if (!IsRaycasted(ref hitPoint, ref norm, _camerasHandler.ActiveCamera, 999999f, true))
                    return;

                float angle = _utilsHandler.RadToDegrees(_utilsHandler.GetAngleBetweenVectors(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position));
                float sign = MathF.Sign(_utilsHandler.GetDotProduct(norm, _utilsHandler.GetCrossProduct(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position)));
                angle *= sign;
                obj.MovingRotation = _objectRotationGetter(obj, angle);
            }
            else
            {
                var test1 = _camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
                var test2 = _utilsHandler.GetWorldCoordFromScreenCoord(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY(), _camerasHandler.ActiveCamera);
                var test3 = test2 - test1;
                var from = test1 + test3 * 0.05f;
                var to = test1 + test3 * 1000f;

                var from1 = _objectPositionFromGetter(obj);
                var to1 = _objectPositionToGetter(obj);
                RAGE.Game.Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                RAGE.Game.Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                Tuple<Vector3, Vector3> drawMe = _utilsHandler.ClosestDistanceBetweenLines(from, to, from1, to1);
                obj.MovingPosition = drawMe.Item2 - (Marker.Position - obj.MovingPosition);
            }
        }

        public void SetHighlighted(Vector3 highlightStartHitPoint)
        {
            HighlightStartHitPoint = highlightStartHitPoint;
            Marker.Color = Color.FromArgb(255, 255, 0);
        }

        public void SetNotHighlighted()
        {
            Marker.Color = _originalColor;
        }

        private void CheckClosestMoveMarker(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            if (IsSphereCasted(_camerasHandler.ActiveCamera))
            {
                float dist = Marker.Position.DistanceTo(_camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord());
                if (dist < closestDist || closestMarker?.IsRotationMarker == true)
                {
                    closestDist = dist;
                    closestMarker = this;
                    hitPointClosestMarker = Marker.Position;
                }
            }
        }

        private void CheckClosestRotateMarker(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            Vector3 hitPoint = new Vector3();
            Vector3 norm = new Vector3();
            if (IsRaycasted(ref hitPoint, ref norm, _camerasHandler.ActiveCamera))
            {
                float dist = hitPoint.DistanceTo(_camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestMarker = this;
                    hitPointClosestMarker = hitPoint;
                }
            }
        }
    }
}
