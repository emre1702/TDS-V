using RAGE;
using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Instance.MapCreator;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;
using Entity = RAGE.Game.Entity;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectPlacing
    {
#pragma warning disable IDE1006 // Naming Styles
        private const bool ONLY_HOLD_OWN_OBJECTS = true;
#pragma warning restore IDE1006 // Naming Styles

        public static MapCreatorObject LastHighlightedObject;
        public static MapCreatorObject HighlightedObject
        {
            get => _highlightedObject;
            set
            {
                _highlightedObject = value;
                if (value != null)
                    LastHighlightedObject = value;
            }
        }
        public static MapCreatorObject HoldingObject;

        private static MapCreatorObject _highlightedObject;
        private static float _clampDistance = 50f;
        private static bool _placeOnGround = true;

        public static void OnTick()
        {
            if (MarkerManager.ClickedMarker != null)
                return;

            if (HoldingObject == null && CursorManager.Visible)
                HighlightObject();
            else if (HoldingObject != null)
                MoveHoldingObject();

            if (HighlightedObject != null) 
                Draw.DrawSkeleton(HighlightedObject.Position, HighlightedObject.Size, HighlightedObject.Rotation);
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
                    string objName = (string)editingTeamIndexOrObjectName;
                    uint objectHash = Misc.GetHashKey(objName);
                    obj = ObjectsManager.GetObject(objectHash, EMapCreatorPositionType.Object, objName);
                    ObjectPreview.Stop();
                    break;
            }

            if (obj == null)
                return;
            HoldingObject = null;
            HighlightedObject = obj;
            HoldHighlightingObject();

        }

        public static void HighlightObjectWithId(int id)
        {
            if (HoldingObject != null)
                return;

            if (id == -1)
                HighlightedObject = null;
            else
            {
                var obj = ObjectsManager.GetByID(id);
                if (obj == null || obj.Entity.IsNull)
                    return;

                HighlightedObject = obj;
            }
        }

        public static void HoldObjectWithID(int id)
        {
            var obj = ObjectsManager.GetByID(id);
            if (obj == null || obj.Entity.IsNull)
                return;

            if (HoldingObject != null)
                ReleaseObject();

            HoldingObject = null;
            HighlightedObject = obj;
            HoldHighlightingObject();
        }

        public static void LeftMouseClick(Control _)
        {
            if (MarkerManager.ClickedMarker != null)
                return;

            if (HoldingObject == null && HighlightedObject != null)
                HoldHighlightingObject();
            else if (HoldingObject != null)
                ReleaseObject();
        }

        public static void DeleteHoldingObject(EKey _)
        {
            if (HoldingObject == null)
                return;
            Browser.Angular.Main.RemovePositionInMapCreatorBrowser(HoldingObject.ID, HoldingObject.Type);
            HoldingObject.Delete();
            if (HighlightedObject == HoldingObject)
                HighlightedObject = null;
            HoldingObject = null;
        }

        public static void CheckObjectDeleted()
        {
            if (HoldingObject?.Deleted == true)
                HoldingObject = null;
            if (HighlightedObject?.Deleted == true)
                HighlightedObject = null;
            if (LastHighlightedObject?.Deleted == true)
                LastHighlightedObject = null;
        }

        private static void HoldHighlightingObject()
        {
            HoldingObject = HighlightedObject;
            HighlightedObject = null;
            HoldingObject.LoadEntityData();

            Draw.HighlightColor_Edge = new RGBA(255, 255, 0, 35);
            Draw.HighlightColor_Full = new RGBA(255, 255, 0, 35);
        }

        private static void ReleaseObject()
        {
            HoldingObject.Position = HoldingObject.MovingPosition;
            HoldingObject.Rotation = HoldingObject.MovingRotation;
            var obj = HoldingObject;
            HoldingObject = null;
            object info = null;
            if (obj.Type == EMapCreatorPositionType.TeamSpawn)
                info = obj.TeamNumber;
            else if (obj.Type == EMapCreatorPositionType.Object)
                info = obj.ObjectName;
            Browser.Angular.Main.AddPositionToMapCreatorBrowser(obj.ID, obj.Type, obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, info);

            Draw.HighlightColor_Edge = new RGBA(255, 255, 255, 35);
            Draw.HighlightColor_Full = new RGBA(255, 255, 255, 35);
        }

        private static void MoveHoldingObject()
        {
            if (CursorManager.Visible)
                MoveHoldingObjectWithCursor();
            else
                MoveHoldingObjectWithCamera();

            if (_placeOnGround)
                PlaceOnGround(HoldingObject);
            HoldingObject.ActivatePhysics();
        }

        private static void MoveHoldingObjectWithCursor()
        {
            if (Pad.IsDisabledControlJustPressed(0, (int)Control.CursorScrollUp))
            {
                _clampDistance += 5f;
                if (_clampDistance > 500f)
                    _clampDistance = 500f;
            }
            else if (Pad.IsDisabledControlJustReleased(0, (int)Control.CursorScrollDown))
            {
                _clampDistance -= 5f;
                if (_clampDistance < 5f)
                    _clampDistance = 5;
            }

            Vector3 camPos = CameraManager.FreeCam.Position;
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

        private static void MoveHoldingObjectWithCamera()
        {
            var hit = GetCameraHit(1000, HoldingObject.Entity.Handle, -1);
            Vector3 camPos = CameraManager.FreeCam.Position;
            if (hit.Item1.Hit && hit.Item1.EndCoords.DistanceTo(camPos) <= _clampDistance)
            {
                HoldingObject.MovingPosition = hit.Item1.EndCoords;
            }
            else
            {
                HoldingObject.MovingPosition = hit.Item2;
            }
        }

        private static void HighlightObject()
        {
            var newHighlightedObject = GetHighlightingObject();

            if (newHighlightedObject != HighlightedObject)
            {
                Chat.Output("HighlightObject");
                RemoveHightlightObject(HighlightedObject);

                if (newHighlightedObject != null)
                    AddHightlightObject(newHighlightedObject);
            }

            HighlightedObject = newHighlightedObject;
        }

        private static void AddHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            obj.LoadEntityData();
            Entity.SetEntityAlpha(obj.Entity.Handle, 180, false);
        }

        private static void RemoveHightlightObject(MapCreatorObject obj)
        {
            if (obj == null)
                return;
            Entity.SetEntityAlpha(obj.Entity.Handle, 255, false);
        }

        private static MapCreatorObject GetHighlightingObject()
        {
            var hit = GetCursorHit(300, RAGE.Elements.Player.LocalPlayer.Handle, 8 | 16).Item1;
            if (!hit.Hit)
                return null;
            if (hit.EntityHit == 0)
                return null;

            var obj = ONLY_HOLD_OWN_OBJECTS ? ObjectsManager.GetByHandle(hit.EntityHit) : ObjectsManager.GetOrCreateByHandle(hit.EntityHit);
            if (obj == null || obj.Entity.IsNull)
                return null;

            return obj;
        }

        public static void TogglePlaceOnGround(EKey _)
        {
            _placeOnGround = !_placeOnGround;
        }

        private static (Raycasting.RaycastHit, Vector3) GetCursorHit(float toDistance, int ignoreHandle, int flags)
        {
            Vector3 camPos = CameraManager.FreeCam.Position;
            Vector3 cursorPos = ClientUtils.GetWorldCoordFromScreenCoord(ClientUtils.GetCursorX(), ClientUtils.GetCursorY(), CameraManager.FreeCam);
            Vector3 difference = cursorPos - camPos;
            Vector3 from = camPos + difference * 0.05f;
            Vector3 to = camPos + difference * toDistance;

            Vector3 t = to - from;
            t.Normalize();
            t *= _clampDistance;
            Vector3 v = camPos + t;

            return (Raycasting.RaycastFromTo(from, to, ignoreHandle, flags), v);
        }

        private static (Raycasting.RaycastHit, Vector3) GetCameraHit(float toDistance, int ignoreHandle, int flags)
        {
            Vector3 camPos = CameraManager.FreeCam.Position;
            Vector3 lookingAtPos = ClientUtils.GetWorldCoordFromScreenCoord(0.5f, 0.5f, CameraManager.FreeCam);
            Vector3 difference = lookingAtPos - camPos;
            Vector3 from = camPos + difference * 0.05f;
            Vector3 to = camPos + difference * toDistance;

            Vector3 t = to - from;
            t.Normalize();
            t *= _clampDistance;
            Vector3 v = camPos + t;

            return (Raycasting.RaycastFromTo(from, to, ignoreHandle, flags), v);
        }

        private static void PlaceOnGround(MapCreatorObject obj)
        {
            switch (obj.Entity.Type)
            {
                case Type.Object:
                    Object.PlaceObjectOnGroundProperly(obj.Entity.Handle);
                    break;
                case Type.Vehicle:
                    RAGE.Game.Vehicle.SetVehicleOnGroundProperly(obj.Entity.Handle, 0);
                    break;
                case Type.Ped:
                    float heightAboveGround = Entity.GetEntityHeightAboveGround(obj.Entity.Handle);
                    obj.MovingPosition = new Vector3(obj.MovingPosition.X, obj.MovingPosition.Y, obj.MovingPosition.Z - heightAboveGround + 1f);
                    break;
            }
        }
    }
}
