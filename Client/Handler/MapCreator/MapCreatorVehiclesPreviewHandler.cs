﻿using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorVehiclesPreviewHandler
    {
        private IVehicle _vehicle;
        private Position3D _vehicleRotation;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly IModAPI ModAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly BrowserHandler _browserHandler;

        public MapCreatorVehiclesPreviewHandler(IModAPI modAPI, CamerasHandler camerasHandler, UtilsHandler utilsHandler, BrowserHandler browserHandler)
        {
            ModAPI = modAPI;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _browserHandler = browserHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(RenderVehicleInFrontOfCam);

            modAPI.Event.Add(FromBrowserEvent.MapCreatorStartVehicleChoice, Start);
            modAPI.Event.Add(FromBrowserEvent.MapCreatorShowVehicle, args => ShowVehicle((string)args[0]));
            modAPI.Event.Add(FromBrowserEvent.MapCreatorStopVehiclePreview, _ => Stop());
        }

        public void ShowVehicle(string vehicleName)
        {
            var hash = ModAPI.Misc.GetHashKey(vehicleName);
            if (hash == default)
                return;

            if (_vehicle == null)
                ModAPI.Event.Tick.Add(_tickEventMethod);
            else
                _vehicle.Destroy();

            _vehicleRotation = new Position3D();
            _vehicle = ModAPI.Vehicle.Create(hash, ModAPI.LocalPlayer.Position, _vehicleRotation, dimension: ModAPI.LocalPlayer.Dimension);
            _vehicle.SetCollision(false, false);
            _vehicle.SetInvincible(true);
            _vehicle.Rotation = _vehicleRotation;
        }

        private void Start(object[] args)
        {
            _browserHandler.MapCreatorVehicleChoice.CreateBrowser();
            _browserHandler.MapCreatorVehicleChoice.SetReady();
        }

        public void Stop()
        {
            if (_vehicle != null)
            {
                _vehicle.Destroy();
                _vehicle = null;
                _vehicleRotation = null;
                ModAPI.Event.Tick.Remove(_tickEventMethod);
            }
            _browserHandler.MapCreatorVehicleChoice.Stop();
        }

        private void RenderVehicleInFrontOfCam(int currentMs)
        {
            var camPos = _camerasHandler.ActiveCamera?.Position ?? ModAPI.Cam.GetGameplayCamCoord();
            var camDirection = _camerasHandler.ActiveCamera?.Direction ?? _utilsHandler.GetDirectionByRotation(ModAPI.Cam.GetGameplayCamRot());

            Position3D a = new Position3D();
            Position3D b = new Position3D();
            _vehicle.GetModelDimensions(a, b);
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

            _vehicle.Rotation = _vehicleRotation;
        }
    }
}