using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorObjectsPreviewHandler
    {
        private IMapObject _object;
        private Position3D _objectRotation;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly IModAPI _modAPI;
        private readonly ObjectsLoadingHelper _objectLoadingHelper;
        private readonly CamerasHandler _camerasHandler;
        private readonly UtilsHandler _utilsHandler;

        public MapCreatorObjectsPreviewHandler(IModAPI modAPI, ObjectsLoadingHelper objectLoadingHelper, CamerasHandler camerasHandler, UtilsHandler utilsHandler)
        {
            _modAPI = modAPI;
            _objectLoadingHelper = objectLoadingHelper;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(RenderObjectInFrontOfCam);
        }

        public void ShowObject(string objectName)
        {
            var hash = _modAPI.Misc.GetHashKey(objectName);
            if (!_objectLoadingHelper.LoadObjectHash(hash))
                return;

            if (_object == null)
                _modAPI.Event.Tick.Add(_tickEventMethod);
            else
                _object.Destroy();

            _objectRotation = new Position3D();
            _object = _modAPI.MapObject.Create(hash, _modAPI.LocalPlayer.Position, _objectRotation, dimension: _modAPI.LocalPlayer.Dimension);
            _object.SetCollision(false, false);
            _object.SetInvincible(true);
        }

        public void Stop()
        {
            if (_object != null)
            {
                _object.Destroy();
                _object = null;
                _objectRotation = null;
                _modAPI.Event.Tick.Remove(_tickEventMethod);
            }
        }

        private void RenderObjectInFrontOfCam(ulong currentMs)
        {
            var camPos = _camerasHandler.ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
            var camDirection = _camerasHandler.ActiveCamera?.Direction ?? _utilsHandler.GetDirectionByRotation(_modAPI.Cam.GetGameplayCamRot());

            Position3D a = new Position3D();
            Position3D b = new Position3D();
            _modAPI.Misc.GetModelDimensions(_object.Model, a, b);
            var objSize = b - a;
            var position = camPos + camDirection * (3 + objSize.Length());
            _object.Position = position;

            _objectRotation.X += 0.1f;
            if (_objectRotation.X >= 360)
                _objectRotation.X -= 360;
            _objectRotation.Y += 0.2f;
            if (_objectRotation.Y >= 360)
                _objectRotation.Y -= 360;
            _objectRotation.Z += 0.5f;
            if (_objectRotation.Z >= 360)
                _objectRotation.Z -= 360;

            _object.Rotation = _objectRotation;
        }
    }
}
