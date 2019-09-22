using RAGE;
using RAGE.Game;
using System;
using System.Linq;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;

namespace TDS_Client.Manager.MapCreator
{
    class MarkerManager
    {
        private static AxisMarker[] _rotateMarker;
        public static AxisMarker ClickedMarker;
        private static AxisMarker _highlightedMarker;

        public static void Start()
        {
            _rotateMarker = new AxisMarker[]
            {
                new AxisMarker(27, new RGBA(255, 0, 0), AxisMarker.AxisEnum.X,
                    rotationGetter: obj => new Vector3(-obj.MovingRotation.Z + 90f, 90, 0),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X, obj.Rotation.Y + angle, obj.Rotation.Z)),     // Marker_X_Rotate

                new AxisMarker(27, new RGBA(0, 255, 0), AxisMarker.AxisEnum.Y,
                    rotationGetter: obj => new Vector3(-obj.MovingRotation.Z, 90, 0),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X - angle, obj.Rotation.Y, obj.Rotation.Z)),     // Marker_Y_Rotate

                new AxisMarker(27, new RGBA(0, 0, 255), AxisMarker.AxisEnum.Z,
                    rotationGetter: obj => new Vector3(0, 0, 0),
                    objectRotationGetter: (obj, angle) => new Vector3(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z - angle)),     // Marker_Z_Rotate

                new AxisMarker(28, new RGBA(255, 0, 0), AxisMarker.AxisEnum.X,
                    positionGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, obj.Size.X / 2f + (obj.Size.X / 4f), 0f, 0f),
                    objectPositionFromGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, -1000f, 0f, 0f),
                    objectPositionToGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 1000f, 0f, 0f)),    // Marker_X_Move

                new AxisMarker(28, new RGBA(0, 255, 0),AxisMarker.AxisEnum.Y,
                    positionGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, obj.Size.Y / 2f + (obj.Size.Y / 4f), 0f),
                    objectPositionFromGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0, -1000f, 0f),
                    objectPositionToGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, 1000f, 0f)),    // Marker_Y_Move

                new AxisMarker(28, new RGBA(0, 0, 255),AxisMarker.AxisEnum.Z,
                    positionGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, 0f, obj.Size.Z + (obj.Size.Z / 4f)),
                    objectPositionFromGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, 0f, -1000f),
                    objectPositionToGetter: obj => Entity.GetOffsetFromEntityInWorldCoords(obj.Entity.Handle, 0f, 0f, 1000f)),    // Marker_Z_Move
            };
            ClickedMarker = null;
        }

        public static void Stop()
        {
            _rotateMarker = null;
            ClickedMarker = null;
        }

        public static void OnTick()
        {
            if (_rotateMarker == null)
                return;

            if (ObjectPlacing.LastHighlightedObject == null || ObjectPlacing.HoldingObject != null)
                return;

            var obj = ObjectPlacing.LastHighlightedObject;
            float rotateScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) * 1.25f;
            float moveScale = Math.Max(Math.Max(obj.Size.X, obj.Size.Y), obj.Size.Z) / 15f;

            if (ClickedMarker != null)
            {
                if (Pad.IsDisabledControlJustReleased(0, (int)Control.Attack))
                {
                    ClickedMarker = null;
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
                Vector3 hitPointClosestMarker = null;
                switch (obj.Type)
                {
                    case EMapCreatorPositionType.BombPlantPlace:
                    case EMapCreatorPositionType.MapCenter:
                    case EMapCreatorPositionType.MapLimit:
                        foreach (var marker in _rotateMarker.Where(m => m.IsPositionMarker))
                        {
                            marker.LoadObjectData(obj, marker.IsRotationMarker ? rotateScale : moveScale);
                            marker.CheckClosest(ref closestDistToMarker, ref closestMarker, ref hitPointClosestMarker);
                        }
                        break;
                    case EMapCreatorPositionType.TeamSpawn:
                        foreach (var marker in _rotateMarker.Where(m => m.IsPositionMarker || m.Axis == AxisMarker.AxisEnum.Z))
                        {
                            marker.LoadObjectData(obj, marker.IsRotationMarker ? rotateScale : moveScale);
                            marker.CheckClosest(ref closestDistToMarker, ref closestMarker, ref hitPointClosestMarker);
                        }
                        break;
                    default:
                        foreach (var marker in _rotateMarker)
                        {
                            marker.LoadObjectData(obj, marker.IsRotationMarker ? rotateScale : moveScale);
                            marker.CheckClosest(ref closestDistToMarker, ref closestMarker, ref hitPointClosestMarker);
                        }
                        break;
                }
                

                if (_highlightedMarker != closestMarker)
                {
                    _highlightedMarker?.SetNotHighlighted();
                    _highlightedMarker = closestMarker;
                    _highlightedMarker?.SetHighlighted(hitPointClosestMarker);
                }

                if (_highlightedMarker != null && _highlightedMarker.IsPositionMarker)
                    // false comes first, so if we want to have RotationMarker first, we have to use ! before it
                    foreach (var marker in _rotateMarker.OrderBy(m => !m.IsRotationMarker))  
                        marker.Draw();
                else
                    // false comes first, so if we want to have PositionMarker first, we have to use ! before it
                    foreach (var marker in _rotateMarker.OrderBy(m => !m.IsPositionMarker))
                        marker.Draw();
            }

            if (Pad.IsDisabledControlJustPressed(0, (int)Control.Attack))
            {
                ClickedMarker = _highlightedMarker;
                Draw.HighlightColor_Edge = new RGBA(255, 255, 0, 255);
                Draw.HighlightColor_Full = new RGBA(255, 255, 0, 35);
            }
            else if (ObjectPlacing.HighlightedObject == null)
            {
                Draw.HighlightColor_Edge = new RGBA(255, 255, 255, 255);
                Draw.HighlightColor_Full = new RGBA(255, 255, 255, 35);
            }
        }

    }
}
