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
        private static float _currentScrollSpeed = 1f;
        private static bool _isUpPressed;
        private static bool _isDownPressed;

        public static void Start()
        {
            TickManager.Add(OnTick);
            BindManager.Add(EKey.E, KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.E, KeyUp, EKeyPressState.Up);
            BindManager.Add(EKey.Q, KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.Q, KeyUp, EKeyPressState.Up);

            var player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(true);
            player.SetInvincible(true);
            player.SetVisible(false, false);
            player.SetCollision(false, false);

            var cam = CameraManager.FreeCam;
            cam.SetPosition(Cam.GetGameplayCamCoord());
            cam.Rotation = Cam.GetGameplayCamRot(2);
            Cam.SetCamFov(cam.Handle, Cam.GetGameplayCamFov());
            cam.Activate();
            cam.Render();

            var lang = Settings.Language;          
            InstructionalButtonManager.Add(lang.SLOWER, Control.VehicleFlySelectPrevWeapon);
            InstructionalButtonManager.Add(lang.FASTER, Control.VehicleFlySelectNextWeapon);
            InstructionalButtonManager.Add(lang.SLOW_MODE, lang.LEFT_CTRL);
            InstructionalButtonManager.Add(lang.FAST_MODE, lang.LEFT_SHIFT);
            InstructionalButtonManager.Add(lang.DOWN, "E");
            InstructionalButtonManager.Add(lang.UP, "Q");
            InstructionalButtonManager.Add(lang.DIRECTION, Control.VehicleRoof);
            InstructionalButtonManager.IsLayoutPositive = false;
            InstructionalButtonManager.IsActive = true;
        }

        public static void Stop()
        {
            TickManager.Remove(OnTick);
            BindManager.Remove(EKey.E, KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.E, KeyUp, EKeyPressState.Up);
            BindManager.Remove(EKey.Q, KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.Q, KeyUp, EKeyPressState.Up);

            CameraManager.FreeCam.Deactivate();
            TDSCamera.RenderBack();

            var player = RAGE.Elements.Player.LocalPlayer;
            player.SetInvincible(false);
            player.SetVisible(true, false);
            player.SetCollision(true, true);

            InstructionalButtonManager.Reset();
            InstructionalButtonManager.IsActive = false;
        }

        private static void OnTick()
        {
            Ui.HideHudComponentThisFrame((int)EHudComponent.HUD_WEAPON_WHEEL);

            if (!CursorManager.Visible)
                MoveCam();
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

            cam.SetPosition(pos.X - vector.X + rightVector.X, pos.Y - vector.Y + rightVector.Y, pos.Z - vector.Z + rightVector.Z + goUp - goDown);
            var camPos = cam.Position;
            Streaming.SetFocusArea(camPos.X, camPos.Y, camPos.Z, 0, 0, 0);
            if (Pad.IsControlPressed(0, (int)Control.Aim))
            {
                float rotX = Math.Max(Math.Min(rot.X + rightAxisY * -5f, 89), -89);
                cam.Rotation = new Vector3(rotX, 0.0f, rot.Z + rightAxisX * -5f);
            }

        }

        private static void KeyDown(EKey key)
        {
            switch (key)
            {
                case EKey.E:
                    _isUpPressed = true;
                    break;
                case EKey.Q:
                    _isDownPressed = true;
                    break;
            }
        }

        private static void KeyUp(EKey key)
        {
            switch (key)
            {
                case EKey.E:
                    _isUpPressed = false;
                    break;
                case EKey.Q:
                    _isDownPressed = false;
                    break;
            }
        }
    }
}
