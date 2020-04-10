using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFreecamHandler
    {
        public bool IsActive;

        private float _currentScrollSpeed = 1f;
        private bool _isUpPressed;
        private bool _isDownPressed;

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly SpectatingHandler _spectatingHandler;
        private readonly MapCreatorBindsHandler _mapCreatorBindsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly MapCreatorFootHandler _mapCreatorFootHandler;

        public MapCreatorFreecamHandler(IModAPI modAPI, CamerasHandler camerasHandler, UtilsHandler utilsHandler, SpectatingHandler spectatingHandler,
            MapCreatorBindsHandler mapCreatorBindsHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler,
            CursorHandler cursorHandler, BrowserHandler browserHandler, MapCreatorFootHandler mapCreatorFootHandler)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _spectatingHandler = spectatingHandler;
            _mapCreatorBindsHandler = mapCreatorBindsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;
            _browserHandler = browserHandler;
            _mapCreatorFootHandler = mapCreatorFootHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(OnTick);
        }

        public void Start()
        {
            IsActive = true;
            _modAPI.Event.Tick.Add(_tickEventMethod);

            if (_camerasHandler.FreeCam is null)
                _camerasHandler.FreeCam = new TDSCamera(_modAPI, _camerasHandler, _utilsHandler, _spectatingHandler);


            var cam = _camerasHandler.FreeCam;
            cam.Position = _modAPI.Cam.GetGameplayCamCoord();
            cam.Rotation = _modAPI.Cam.GetGameplayCamRot();
            cam.SetFov(_modAPI.Cam.GetGameplayCamFov());

            cam.Activate();
            cam.Render();

            _mapCreatorBindsHandler.SetForInFreecam();

            _instructionalButtonHandler.Add(_settingsHandler.Language.ON_FOOT, "M");
        }

        public void Stop()
        {
            IsActive = false;
            _modAPI.Event.Tick.Remove(_tickEventMethod);

            _camerasHandler.FreeCam?.Deactivate();
            _camerasHandler.FreeCam = null;


            _mapCreatorBindsHandler.RemoveForInFreeCam();
        }

        private void OnTick(ulong _)
        {
            _modAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_WHEEL);

            if (!_cursorHandler.Visible)
                MoveCam();

            MapCreatorMarkerHandler.OnTick();
            MapCreatorObjectPlacingHandler.OnTick();
        }

        private void MoveCam()
        {
            var cam = _camerasHandler.FreeCam;

            Position3D pos = cam.Position;
            Position3D dir = cam.Direction;
            Position3D rot = cam.Rotation;

            float rightAxisX = _modAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptRightAxisX) * 2f; //behave weird, fix
            float rightAxisY = _modAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptRightAxisY) * 2f;

            float leftAxisX = _modAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptLeftAxisX);
            float leftAxisY = _modAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptLeftAxisY);

            float slowMult = _modAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Duck) ? 0.5f : 1f;
            float fastMult = _modAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Sprint) ? 3f : 1f;

            if (_modAPI.Control.IsControlJustReleased(InputGroup.MOVE, Control.CursorScrollUp))
                _currentScrollSpeed *= 2f;
            else if (_modAPI.Control.IsControlJustReleased(InputGroup.MOVE, Control.CursorScrollDown))
                _currentScrollSpeed /= 2f;

            Position3D vector = new Position3D
            {
                X = dir.X * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Y = dir.Y * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Z = dir.Z * leftAxisY * slowMult * fastMult * _currentScrollSpeed
            };
            Position3D upVector = new Position3D(0, 0, 1);
            Position3D rightVector = _utilsHandler.GetCrossProduct(dir.Normalized, upVector.Normalized); // Is this the same as * ?

            rightVector.X *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Y *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Z *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;

            float goUp = _isUpPressed ? 0.5f : 0f;
            float goDown = _isDownPressed ? 0.5f : 0f;

            Position3D newPos = new Position3D(pos.X - vector.X + rightVector.X, pos.Y - vector.Y + rightVector.Y, pos.Z - vector.Z + rightVector.Z + goUp - goDown);
            cam.SetPosition(newPos);
            _modAPI.LocalPlayer.Position = newPos;
            if (_modAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Aim))
            {
                float rotX = Math.Max(Math.Min(rot.X + rightAxisY * -5f, 89), -89);
                var newRot = new Position3D(rotX, 0.0f, rot.Z + rightAxisX * -5f);
                cam.Rotation = newRot;
                _modAPI.LocalPlayer.Rotation = newRot;
            }

        }

        public void KeyDown(Key key)
        {
            if (_browserHandler.InInput)
                return;

            switch (key)
            {
                case Key.E:
                    _isDownPressed = true;
                    break;
                case Key.Q:
                    _isUpPressed = true;
                    break;
            }
        }

        public void KeyUp(Key key)
        {
            switch (key)
            {
                case Key.E:
                    _isDownPressed = false;
                    break;
                case Key.Q:
                    _isUpPressed = false;
                    break;
            }
        }

        public void ToggleFreecam(Key _ = Key.NoName)
        {
            if (_browserHandler.InInput)
                return;

            _instructionalButtonHandler.Reset();
            _mapCreatorBindsHandler.SetGeneral();
            if (IsActive)
            {
                Stop();
                _mapCreatorFootHandler.Start();
            }
            else
            {
                _mapCreatorFootHandler.Stop();
                Start();
            }
        }
    }
}
