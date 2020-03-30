using RAGE;
using RAGE.Elements;
using System;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    static class VehiclePreview
    {
        private static Vehicle _vehicle;
        private static Vector3 _vehicleRotation;

        public static void ShowVehicle(string vehicleName)
        {
            var hash = RAGE.Game.Misc.GetHashKey(vehicleName);
            if (hash == default)
                return;

            if (_vehicle == null)
                TickManager.Add(RenderVehicleInFrontOfCam);
            else
                _vehicle.Destroy();

            _vehicleRotation = new Vector3();
            _vehicle = new Vehicle(hash, Player.LocalPlayer.Position, _vehicleRotation.Z, dimension: Player.LocalPlayer.Dimension);
            _vehicle.SetCollision(false, false);
            _vehicle.SetInvincible(true);
            _vehicle.SetRotation(_vehicleRotation.X, _vehicleRotation.Y, _vehicleRotation.Z, 2, true);
        }

        public static void Stop()
        {
            if (_vehicle != null)
            {
                _vehicle.Destroy();
                _vehicle = null;
                _vehicleRotation = null;
                TickManager.Remove(RenderVehicleInFrontOfCam);
            }
        }

        private static void RenderVehicleInFrontOfCam()
        {
            var camPos = TDSCamera.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var camDirection = TDSCamera.ActiveCamera?.Direction ?? ClientUtils.GetDirectionByRotation(RAGE.Game.Cam.GetGameplayCamRot(2));

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
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
    }
}
