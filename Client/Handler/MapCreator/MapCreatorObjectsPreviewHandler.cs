using RAGE;
using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Entities.GTA;
using TDS.Shared.Data.Models.GTA;
using static RAGE.Events;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorObjectsPreviewHandler
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;

        private readonly ObjectsLoadingHelper _objectLoadingHelper;
        private readonly UtilsHandler _utilsHandler;
        private ITDSObject _object;
        private Vector3 _objectRotation;

        public MapCreatorObjectsPreviewHandler(ObjectsLoadingHelper objectLoadingHelper, CamerasHandler camerasHandler, UtilsHandler utilsHandler, BrowserHandler browserHandler)
        {
            _objectLoadingHelper = objectLoadingHelper;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _browserHandler = browserHandler;

            RAGE.Events.Add(FromBrowserEvent.MapCreatorStartObjectChoice, Start);
            RAGE.Events.Add(FromBrowserEvent.MapCreatorShowObject, args => ShowObject((string)args[0]));
            RAGE.Events.Add(FromBrowserEvent.MapCreatorStopObjectPreview, _ => Stop());
        }

        public void ShowObject(string objectName)
        {
            var hash = RAGE.Game.Misc.GetHashKey(objectName);
            if (!_objectLoadingHelper.LoadObjectHash(hash))
                return;

            if (_object == null)
                Tick += RenderObjectInFrontOfCam;
            else
                _object.Destroy();

            _objectRotation = new Vector3();
            _object = new TDSObject(hash, RAGE.Elements.Player.LocalPlayer.Position, _objectRotation, dimension: RAGE.Elements.Player.LocalPlayer.Dimension);
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
                Tick -= RenderObjectInFrontOfCam;
            }
        }

        private void RenderObjectInFrontOfCam(List<TickNametagData> _)
        {
            var camPos = _camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var camDirection = _camerasHandler.ActiveCamera?.Direction ?? _utilsHandler.GetDirectionByRotation(RAGE.Game.Cam.GetGameplayCamRot(2));

            var a = new Vector3();
            var b = new Vector3(9999f, 9999f, 9999f);
            RAGE.Game.Misc.GetModelDimensions(_object.Model, a, b);

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

            _object.SetRotation(_objectRotation.X, _objectRotation.Y, _objectRotation.Z, 2, true);
        }

        private void Start(object[] args)
        {
        }
    }
}