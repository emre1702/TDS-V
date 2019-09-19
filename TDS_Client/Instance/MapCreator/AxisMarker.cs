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
        public enum AxisMarkerMovePosType
        {
            None, X, Y, Z
        }

        public Marker Marker;

        private Func<float, Vector3> _rotationGetter;  
        private AxisMarkerMovePosType _movePosType;


        public AxisMarker(RGBA color, Func<float, Vector3> rotationGetter = null, AxisMarkerMovePosType movePosType = AxisMarkerMovePosType.None)
        {
            Marker = new Marker(_movePosType != AxisMarkerMovePosType.None ? 27 : 28, new Vector3(), new Vector3(), new Vector3(), new Vector3(), color);
            _rotationGetter = rotationGetter;
            _movePosType = movePosType;
        }

        public bool IsRaycasted(ref Vector3 hitPoint, ref Vector3 norm, TDSCamera cam = null, float threshold = 0.1f, bool ignoreDistance = false)
        {
            Vector3 test1 = cam?.Position ?? Cam.GetGameplayCamCoord();
            Vector3 test2 = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY());
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
            Vector3 test2 = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY());
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
                if (Marker.Type == 28)
                    Graphics.DrawSprite("commonmenu", "common_medal", v.X, v.Y, Marker.Scale.X * 4 / dist * ((float)Dx.ResY / (float)Dx.ResX), Marker.Scale.X * 4 / dist, 0, (int)Marker.Color.Red, (int)Marker.Color.Green, (int)Marker.Color.Blue, (int)Marker.Color.Alpha, 0);
            }
        }

        public void HandleClick(MapCreatorObject obj)
        {
            float x = 0f;
            if (_movePosType != AxisMarkerMovePosType.None)
            {
                x = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) * 1.25f;
                Marker.Position = obj.MovingPosition;
                Marker.Rotation = _rotationGetter(obj.MovingRotation.Z);
            }
            else
            {
                switch (_movePosType)
                {
                    case AxisMarkerMovePosType.X:
                        Marker.Position = Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, obj.Size.X / 2f + (obj.Size.X / 4f), 0f, 0f);
                        break;
                    case AxisMarkerMovePosType.Y:
                        Marker.Position = Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, obj.Size.Y / 2f + (obj.Size.Y / 4f), 0f);
                        break;
                    case AxisMarkerMovePosType.Z:
                        Marker.Position = Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, 0f, obj.Size.Z / 2f + (obj.Size.Z / 4f));
                        break;
                }
                x = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) / 15f;

            }

            Marker.Scale = new Vector3(x, x, x);
            Marker.Draw();
        }
    }
}
