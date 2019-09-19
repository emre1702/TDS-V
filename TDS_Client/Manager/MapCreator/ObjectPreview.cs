using RAGE;
using RAGE.Elements;
using System;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    static class ObjectPreview
    {
        private static MapObject _object;
        private static Vector3 _objectRotation;

        public static void ShowObject(string objectName)
        {
            var hash = RAGE.Game.Misc.GetHashKey(objectName);
            if (!ObjectLoading.LoadObjectHash(hash))
                return;

            if (_object == null)
                TickManager.Add(RenderObjectInFrontOfCam);
            else 
                _object.Destroy();

            _objectRotation = new Vector3();
            _object = new MapObject(hash, Player.LocalPlayer.Position, _objectRotation, dimension: Player.LocalPlayer.Dimension);
            _object.SetCollision(false, false);
            _object.SetInvincible(true);
        }

        public static void Stop()
        {
            if (_object != null)
            {
                _object.Destroy();
                _object = null;
                _objectRotation = null;
                TickManager.Remove(RenderObjectInFrontOfCam);
            }
        }

        private static void RenderObjectInFrontOfCam()
        {
            var camPos = TDSCamera.ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
            var camDirection = TDSCamera.ActiveCamera?.Direction ?? ClientUtils.GetDirectionByRotation(RAGE.Game.Cam.GetGameplayCamRot(2));

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
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
    }
}
