using RAGE;
using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Utility;
using Entity = RAGE.Game.Entity;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectPlacing
    {
#pragma warning disable IDE1006 // Naming Styles
        private const bool ONLY_HOLD_OWN_OBJECTS = true;
#pragma warning restore IDE1006 // Naming Styles

        private static MapCreatorObject _highlightedObject;
        private static MapCreatorObject _holdingObject;
        private static float _clampDistance = 50f;
        private static bool _placeOnGround = true;  // TRUE IS DEBUG

        public static void OnTick()
        {
            
            if (_holdingObject == null && CursorManager.Visible)
                HighlightObject();
            else if (_holdingObject != null)
                MoveHoldingObject();

            if (_highlightedObject != null) 
                Draw.DrawSkeleton(_highlightedObject.Position, _highlightedObject.Size, _highlightedObject.Rotation);
        }

        public static void StartNewPlacing(EMapCreatorPositionType type, object editingTeamIndexOrObjectName)
        {
            MapCreatorObject obj = null;
            switch (type)
            {
                case EMapCreatorPositionType.TeamSpawn:
                    obj = ObjectsManager.GetTeamSpawn((int)editingTeamIndexOrObjectName);
                    break;
                case EMapCreatorPositionType.MapCenter:
                    obj = ObjectsManager.GetMapCenter();
                    break;
                case EMapCreatorPositionType.BombPlantPlace:
                    obj = ObjectsManager.GetBombPlantPlace();
                    break;
                case EMapCreatorPositionType.MapLimit:
                    obj = ObjectsManager.GetMapLimit();
                    break;
                case EMapCreatorPositionType.Object:
                    uint objectHash = Misc.GetHashKey((string)editingTeamIndexOrObjectName);
                    obj = ObjectsManager.GetObject(objectHash);
                    break;
            }

            if (obj == null)
                return;
            _holdingObject = null;
            _highlightedObject = obj;
            HoldHighlightingObject();

        }

        public static void LeftMouseClick(Control _)
        {
            if (_holdingObject == null && _highlightedObject != null && CursorManager.Visible)
                HoldHighlightingObject();
            else if (_holdingObject != null && CursorManager.Visible)
                ReleaseObject();
        }

        public static void DeleteHoldingObject(EKey _)
        {
            if (_holdingObject == null)
                return;
            _holdingObject.Delete();
            if (_highlightedObject == _holdingObject)
                _highlightedObject = null;
            _holdingObject = null;
        }

        private static void HoldHighlightingObject()
        {
            Chat.Output("Hold hightlighting object");
            _holdingObject = _highlightedObject;
            _highlightedObject = null;
            _holdingObject.LoadEntityData();
        }

        private static void ReleaseObject()
        {
            Chat.Output($"Release {_holdingObject.MovingPosition.X}|{_holdingObject.MovingPosition.Y}|{_holdingObject.MovingPosition.Z}");
            _holdingObject.Position = _holdingObject.MovingPosition;
            _holdingObject.Rotation = _holdingObject.MovingRotation;
            _holdingObject = null;
            // Trigger the new position to Angular
        }

        private static void MoveHoldingObject()
        {
            if (CursorManager.Visible)
                MoveHoldingObjectWithCursor();
            else
            {

            }
        }

        private static void MoveHoldingObjectWithCursor()
        {
            if (Pad.IsControlJustReleased(0, (int)Control.CursorScrollUp))
            {
                Chat.Output("scroll up");
                _clampDistance += 5f;
                if (_clampDistance > 500f)
                    _clampDistance = 500f;
            }
            else if (Pad.IsControlJustReleased(0, (int)Control.CursorScrollDown))
            {
                Chat.Output("scroll down");
                _clampDistance -= 5f;
                if (_clampDistance < 10f)
                    _clampDistance = 10f;
            }

            Vector3 camPos = CameraManager.FreeCam.Position;
            var hit = GetCursorHit(1000, _holdingObject.Entity.Handle, -1);

            if (hit.Item1.Hit && hit.Item1.EndCoords.DistanceTo(camPos) <= _clampDistance)
            {
                Chat.Output("hit end coords");
                _holdingObject.MovingPosition = hit.Item1.EndCoords;   
            } 
            else
            {
                Chat.Output("world coords");
                _holdingObject.MovingPosition = hit.Item2;
            }

            if (_placeOnGround)
            {
                switch (_holdingObject.Entity.Type)
                {
                    case Type.Object:
                        Object.PlaceObjectOnGroundProperly(_holdingObject.Entity.Handle);
                        break;
                    case Type.Vehicle:
                        RAGE.Game.Vehicle.SetVehicleOnGroundProperly(_holdingObject.Entity.Handle, 0);
                        break;
                    case Type.Ped:
                        float heightAboveGround = Entity.GetEntityHeightAboveGround(_holdingObject.Entity.Handle);
                        _holdingObject.MovingPosition = new Vector3(_holdingObject.MovingPosition.X, _holdingObject.MovingPosition.Y, _holdingObject.MovingPosition.Z - heightAboveGround);
                        break;
                }
            }
        }

        private static void HighlightObject()
        {
            var newHighlightedObject = GetHighlightingObject();

            if (newHighlightedObject != _highlightedObject)
            {
                Chat.Output("HighlightObject");
                RemoveHightlightObject(_highlightedObject);

                if (newHighlightedObject != null)
                    AddHightlightObject(newHighlightedObject);
            }

            _highlightedObject = newHighlightedObject;
        }

        private static void AddHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            Entity.SetEntityAlpha(obj.Entity.Handle, 150, true);
        }

        private static void RemoveHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            Entity.SetEntityAlpha(obj.Entity.Handle, 255, true);
        }
    
        private static MapCreatorObject GetHighlightingObject()
        {
            var hit = GetCursorHit(300, CameraManager.FreeCam.Handle, 8 | 16).Item1;
            if (!hit.Hit)
                return null;
            if (hit.EntityHit == 0)
                return null;

            var obj = ONLY_HOLD_OWN_OBJECTS ? ObjectsManager.GetByHandle(hit.EntityHit) : ObjectsManager.GetOrCreateByHandle(hit.EntityHit);
            if (obj == null || obj.Entity.IsNull || !obj.Entity.Exists)
                return null;

            return obj;
        }

        public static void TogglePlaceOnGround(bool toggle)
        {
            _placeOnGround = toggle;
        }

        private static (Raycasting.RaycastHit, Vector3) GetCursorHit(float toDistance, int ignoreHandle, int flags)
        {
            Vector3 camPos = CameraManager.FreeCam.Position;
            Vector3 cursorPos = ClientUtils.GetWorldCoordFromScreenCoord(GetCursorX(), GetCursorY(), CameraManager.FreeCam);
            Vector3 difference = cursorPos - camPos;
            Vector3 from = camPos + difference * 0.05f;
            Vector3 to = camPos + difference * toDistance;

            Vector3 t = to - from;
            t.Normalize();
            t *= _clampDistance;
            Vector3 v = camPos + t;

            return (Raycasting.RaycastFromTo(from, to, ignoreHandle, flags), v);
        }

        private static float GetCursorX()
        {
            return Pad.GetDisabledControlNormal(0, (int)Control.CursorX);
        }

        private static float GetCursorY()
        {
            return Pad.GetDisabledControlNormal(0, (int)Control.CursorY);
        }
    }
}
