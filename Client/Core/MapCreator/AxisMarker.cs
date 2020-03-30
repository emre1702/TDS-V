using RAGE;
using RAGE.Game;
using System;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Instance.MapCreator
{
    class AxisMarker
    {
        public enum AxisEnum
        {
            X, Y, Z
        }

        public readonly Marker Marker;

        public Vector3 HighlightStartHitPoint;

        public bool IsRotationMarker => _rotationGetter != null;
        public bool IsPositionMarker => _positionGetter != null;
        public AxisEnum Axis;

        private readonly Func<MapCreatorObject, Vector3> _positionGetter;
        private readonly Func<MapCreatorObject, Vector3> _objectPositionFromGetter;
        private readonly Func<MapCreatorObject, Vector3> _objectPositionToGetter;
        private readonly Func<MapCreatorObject, Vector3> _rotationGetter;  
        private readonly Func<MapCreatorObject, float, Vector3> _objectRotationGetter;

        private readonly RGBA _originalColor;
        

        public AxisMarker(
            int type,
            RGBA color, 
            AxisEnum axis,
            Func<MapCreatorObject, Vector3> positionGetter = null,
            Func<MapCreatorObject, Vector3> objectPositionFromGetter = null,
            Func<MapCreatorObject, Vector3> objectPositionToGetter = null,
            Func<MapCreatorObject, Vector3> rotationGetter = null,
            Func<MapCreatorObject, float, Vector3> objectRotationGetter = null)
        {
            _originalColor = color;
            Marker = new Marker(type, new Vector3(), new Vector3(), new Vector3(), new Vector3(), color);
            Axis = axis;
            _positionGetter = positionGetter;
            _objectPositionFromGetter = objectPositionFromGetter;
            _objectPositionToGetter = objectPositionToGetter;
            _rotationGetter = rotationGetter;
            _objectRotationGetter = objectRotationGetter;
        }

        public void SetHighlighted(Vector3 highlightStartHitPoint)
        {
            HighlightStartHitPoint = highlightStartHitPoint;
            Marker.Color = new RGBA(255, 255, 0);
        }

        public void SetNotHighlighted()
        {
            Marker.Color = _originalColor;
        }

        public bool IsRaycasted(ref Vector3 hitPoint, ref Vector3 norm, TDSCamera cam = null, float threshold = 0.1f, bool ignoreDistance = false)
        {
            Vector3 test1 = cam?.Position ?? Cam.GetGameplayCamCoord();
            Vector3 test2 = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY(), cam);
            Vector3 test3 = test2 - test1;
            Vector3 from = test1 + test3 * 0.05f;
            Vector3 to = test1 + test3 * 1000f;

            if (!ignoreDistance)
            {
                if (ClientUtils.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), Marker.Scale.X / 2f, from, to, ref hitPoint, threshold, ref norm))
                    if (Marker.Position.DistanceTo(hitPoint) >= (Marker.Scale.X / 2f) * 0.775f - threshold)
                        return true;
            }
            else
            {
                if (ClientUtils.LineIntersectingCircle(Marker.Position, new Vector3(Marker.Rotation.X - 90f, Marker.Rotation.Y, Marker.Rotation.Z), threshold, from, to, ref hitPoint, 0f, ref norm))
                    return true;
            }
            return false;
        }

        public bool IsSphereCasted(TDSCamera cam = null)
        {
            Vector3 test1 = cam?.Position ?? Cam.GetGameplayCamCoord();
            Vector3 test2 = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY(), cam);
            Vector3 test3 = test2 - test1;
            Vector3 from = test1 + test3 * 0.05f;
            Vector3 to = test1 + test3 * 1000f;

            return ClientUtils.LineIntersectingSphere(from, to, Marker.Position, Marker.Scale.X);
        }

        public void Draw()
        {
            Marker.Draw();
            Vector3 v = ClientUtils.GetScreenCoordFromWorldCoord(Marker.Position);
            if (v != null)
            {
                var camPos = TDSCamera.ActiveCamera?.Position ?? Cam.GetGameplayCamCoord();
                float dist = Marker.Position.DistanceTo(camPos);
                if (IsPositionMarker)
                    Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * (Dx.ResY / Dx.ResX), Marker.Scale.X * 4 / dist, 0, (int)Marker.Color.Red, (int)Marker.Color.Green, (int)Marker.Color.Blue, (int)Marker.Color.Alpha, 0);
            }
        }

        public void MoveOrRotateObject(MapCreatorObject obj)
        {
            if (IsRotationMarker)
            {
                Vector3 hitPoint = new Vector3();
                Vector3 norm = new Vector3();
                if (!IsRaycasted(ref hitPoint, ref norm, TDSCamera.ActiveCamera, 999999f, true))
                    return;

                float angle = ClientUtils.RadToDegrees(ClientUtils.GetAngleBetweenVectors(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position));
                float sign = MathF.Sign(ClientUtils.GetDotProduct(norm, ClientUtils.GetCrossProduct(hitPoint - Marker.Position, HighlightStartHitPoint - Marker.Position)));
                angle *= sign;
                obj.MovingRotation = _objectRotationGetter(obj, angle);
            }
            else
            {
                Vector3 test1 = TDSCamera.ActiveCamera?.Position ?? Cam.GetGameplayCamCoord();
                Vector3 test2 = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY(), TDSCamera.ActiveCamera);
                Vector3 test3 = test2 - test1;
                Vector3 from = test1 + test3 * 0.05f;
                Vector3 to = test1 + test3 * 1000f;

                Vector3 from1 = _objectPositionFromGetter(obj);
                Vector3 to1 = _objectPositionToGetter(obj);
                Graphics.DrawLine(from1.X, from1.Y, from1.Z, to1.X, to1.Y, to1.Z, 255, 0, 0, 255);
                Graphics.DrawLine(from.X, from.Y, from.Z, to.X, to.Y, to.Z, 255, 0, 0, 255);
                Tuple<Vector3, Vector3> drawMe = ClientUtils.ClosestDistanceBetweenLines(from, to, from1, to1);
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

            Marker.Scale = new Vector3(scale, scale, scale);
        }

        public void CheckClosest(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            if (IsRotationMarker)
                CheckClosestRotateMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
            else
                CheckClosestMoveMarker(ref closestDist, ref closestMarker, ref hitPointClosestMarker);
        }

        private void CheckClosestRotateMarker(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            Vector3 hitPoint = new Vector3();
            Vector3 norm = new Vector3();
            if (IsRaycasted(ref hitPoint, ref norm, TDSCamera.ActiveCamera))
            {
                float dist = hitPoint.DistanceTo(TDSCamera.ActiveCamera?.Position ?? Cam.GetGameplayCamCoord());
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestMarker = this;
                    hitPointClosestMarker = hitPoint;
                }
            }
        }

        private void CheckClosestMoveMarker(ref float closestDist, ref AxisMarker closestMarker, ref Vector3 hitPointClosestMarker)
        {
            if (IsSphereCasted(TDSCamera.ActiveCamera))
            {
                float dist = Marker.Position.DistanceTo(TDSCamera.ActiveCamera?.Position ?? Cam.GetGameplayCamCoord());
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
