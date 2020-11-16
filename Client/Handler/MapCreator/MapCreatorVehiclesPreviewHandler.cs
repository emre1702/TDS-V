using RAGE;
using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Entities.GTA;
using static RAGE.Events;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorVehiclesPreviewHandler
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;

        private readonly UtilsHandler _utilsHandler;
        private ITDSVehicle _vehicle;
        private Vector3 _vehicleRotation;

        public MapCreatorVehiclesPreviewHandler(CamerasHandler camerasHandler, UtilsHandler utilsHandler, BrowserHandler browserHandler)
        {
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _browserHandler = browserHandler;

            Add(FromBrowserEvent.MapCreatorStartVehicleChoice, Start);
            Add(FromBrowserEvent.MapCreatorShowVehicle, args => ShowVehicle((string)args[0]));
            Add(FromBrowserEvent.MapCreatorStopVehiclePreview, _ => Stop());
        }

        public void ShowVehicle(string vehicleName)
        {
            var hash = RAGE.Game.Misc.GetHashKey(vehicleName);
            if (hash == default)
                return;

            _vehicleRotation = new Vector3();
            if (_vehicle == null)
                Tick += RenderVehicleInFrontOfCam;
            else
                _vehicle.Destroy();

            _vehicle = new TDSVehicle(hash, RAGE.Elements.Player.LocalPlayer.Position, heading: _vehicleRotation.Z, dimension: RAGE.Elements.Player.LocalPlayer.Dimension);
            _vehicle.SetCollision(false, false);
            _vehicle.SetInvincible(true);
            _vehicle.SetRotation(_vehicleRotation.X, _vehicleRotation.Y, _vehicleRotation.Z, 2, true);
        }

        public void Stop()
        {
            if (_vehicle != null)
            {
                _vehicle.Destroy();
                _vehicle = null;
                _vehicleRotation = null;
                Tick -= RenderVehicleInFrontOfCam;
            }
            _browserHandler.MapCreatorVehicleChoice.Stop();
        }

        private void RenderVehicleInFrontOfCam(List<TickNametagData> _)
        {
            if (_vehicle is null)
                return;
            var camPos = _camerasHandler.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var camDirection = _camerasHandler.ActiveCamera?.Direction ?? _utilsHandler.GetDirectionByRotation(RAGE.Game.Cam.GetGameplayCamRot(2));

            var a = new Vector3();
            var b = new Vector3(9999f, 9999f, 9999f);
            RAGE.Game.Misc.GetModelDimensions(_vehicle.Model, a, b);

            var objSize = b - a;
            var position = camPos + camDirection * (3 + objSize.Length());
            _vehicle.Position = position;

            _vehicleRotation.X += 0.1f;
            if (_vehicleRotation.X >= 360)
                _vehicleRotation.X -= 360;
            _vehicleRotation.Y += 0.2f;
            if (_vehicleRotation.Y >= 360)
                _vehicleRotation.Y -= 360;
            _vehicleRotation.Z += 0.5f;
            if (_vehicleRotation.Z >= 360)
                _vehicleRotation.Z -= 360;

            _vehicle.SetRotation(_vehicleRotation.X, _vehicleRotation.Y, _vehicleRotation.Z, 2, true);
        }

        private void Start(object[] args)
        {
            _browserHandler.MapCreatorVehicleChoice.CreateBrowser();
            _browserHandler.MapCreatorVehicleChoice.SetReady();
        }
    }
}
