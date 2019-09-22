using RAGE;
using RAGE.Game;
using System;
using TDS_Client.Enum;
using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Freecam
    {
        public static bool IsActive;

        private static float _currentScrollSpeed = 1f;
        private static bool _isUpPressed;
        private static bool _isDownPressed;

        public static void Start()
        {
            IsActive = true;
            TickManager.Add(OnTick);
            
            var cam = CameraManager.FreeCam;
            cam.Position = Cam.GetGameplayCamCoord();
            cam.Rotation = Cam.GetGameplayCamRot(2);
            Cam.SetCamFov(cam.Handle, Cam.GetGameplayCamFov());
            cam.Activate();
            cam.Render();

            Binds.SetForInFreecam();
        }

        public static void Stop()
        {
            IsActive = false;
            TickManager.Remove(OnTick);

            CameraManager.FreeCam.Deactivate();
            
            Binds.RemoveForInFreeCam();
        }

        private static void OnTick()
        {
            Ui.HideHudComponentThisFrame((int)EHudComponent.HUD_WEAPON_WHEEL);

            if (!CursorManager.Visible)
                MoveCam();

            MarkerManager.OnTick();
            ObjectPlacing.OnTick();
        }

        private static void MoveCam()
        {
            var cam = CameraManager.FreeCam;

            Vector3 pos = cam.Position;
            Vector3 dir = cam.Direction;
            Vector3 rot = cam.Rotation;

            float rightAxisX = Pad.GetDisabledControlNormal(0, (int)Control.ScriptRightAxisX) * 2f; //behave weird, fix
            float rightAxisY = Pad.GetDisabledControlNormal(0, (int)Control.ScriptRightAxisY) * 2f;

            float leftAxisX = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisX);
            float leftAxisY = Pad.GetDisabledControlNormal(0, (int)Control.ScriptLeftAxisY);

            float slowMult = Pad.IsControlPressed(0, (int)Control.Duck) ? 0.5f : 1f;
            float fastMult = Pad.IsControlPressed(0, (int)Control.Sprint) ? 3f : 1f;

            if (Pad.IsControlJustReleased(0, (int)Control.CursorScrollUp))
                _currentScrollSpeed *= 2f;
            else if (Pad.IsControlJustReleased(0, (int)Control.CursorScrollDown))
                _currentScrollSpeed /= 2f;

            Vector3 vector = new Vector3
            {
                X = dir.X * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Y = dir.Y * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Z = dir.Z * leftAxisY * slowMult * fastMult * _currentScrollSpeed
            };
            Vector3 upVector = new Vector3(0, 0, 1);
            Vector3 rightVector = ClientUtils.GetCrossProduct(dir.Normalized, upVector.Normalized); // Is this the same as * ?

            rightVector.X *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Y *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Z *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;

            float goUp = _isUpPressed ? 0.5f : 0f;
            float goDown = _isDownPressed ? 0.5f : 0f;

            Vector3 newPos = new Vector3(pos.X - vector.X + rightVector.X, pos.Y - vector.Y + rightVector.Y, pos.Z - vector.Z + rightVector.Z + goUp - goDown);
            cam.SetPosition(newPos);
            RAGE.Elements.Player.LocalPlayer.Position = newPos;
            if (Pad.IsControlPressed(0, (int)Control.Aim))
            {
                float rotX = Math.Max(Math.Min(rot.X + rightAxisY * -5f, 89), -89);
                var newRot = new Vector3(rotX, 0.0f, rot.Z + rightAxisX * -5f);
                cam.Rotation = newRot;
                RAGE.Elements.Player.LocalPlayer.SetHeading(newRot.Z);
            }

        }

        public static void KeyDown(EKey key)
        {
            switch (key)
            {
                case EKey.E:
                    _isDownPressed = true;
                    break;
                case EKey.Q:
                    _isUpPressed = true;
                    break;
            }
        }

        public static void KeyUp(EKey key)
        {
            switch (key)
            {
                case EKey.E:
                    _isDownPressed = false;
                    break;
                case EKey.Q:
                    _isUpPressed = false;
                    break;
            }
        }
    }
}
