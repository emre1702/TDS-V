using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFreecamHandler : ServiceBase
    {
        #region Public Fields

        public bool IsActive;

        #endregion Public Fields

        #region Private Fields

        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly MapCreatorFootHandler _mapCreatorFootHandler;
        private readonly MapCreatorMarkerHandler _mapCreatorMarkerHandler;
        private readonly MapCreatorObjectPlacingHandler _mapCreatorObjectPlacingHandler;
        private readonly EventMethodData<TickDelegate> _tickEventMethod;
        private readonly UtilsHandler _utilsHandler;
        private float _currentScrollSpeed = 1f;
        private bool _isDownPressed;
        private bool _isUpPressed;

        #endregion Private Fields

        #region Public Constructors

        public MapCreatorFreecamHandler(IModAPI modAPI, LoggingHandler loggingHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler,
            InstructionalButtonHandler instructionalButtonHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, MapCreatorFootHandler mapCreatorFootHandler,
            MapCreatorMarkerHandler mapCreatorMarkerHandler, MapCreatorObjectPlacingHandler mapCreatorObjectPlacingHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _cursorHandler = cursorHandler;
            _browserHandler = browserHandler;
            _mapCreatorFootHandler = mapCreatorFootHandler;
            _mapCreatorMarkerHandler = mapCreatorMarkerHandler;
            _mapCreatorObjectPlacingHandler = mapCreatorObjectPlacingHandler;
            _eventsHandler = eventsHandler;

            _tickEventMethod = new EventMethodData<TickDelegate>(OnTick);
        }

        #endregion Public Constructors

        #region Public Methods

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

        public void Start()
        {
            IsActive = true;
            ModAPI.Event.Tick.Add(_tickEventMethod);

            if (_camerasHandler.FreeCam is null)
                _camerasHandler.FreeCam = new TDSCamera("FreeCam", ModAPI, Logging, _camerasHandler, _utilsHandler);

            var player = ModAPI.LocalPlayer;
            player.FreezePosition(true);
            player.SetVisible(false);
            player.SetCollision(false, false);

            var cam = _camerasHandler.FreeCam;
            cam.Position = ModAPI.Cam.GetGameplayCamCoord();
            cam.Rotation = ModAPI.Cam.GetGameplayCamRot();
            cam.SetFov(ModAPI.Cam.GetGameplayCamFov());

            cam.Activate();
            cam.Render();

            ModAPI.Sync.TriggerEvent(ToServerEvent.SetInFreecam, true);
        }

        public void Stop()
        {
            IsActive = false;
            ModAPI.Event.Tick.Remove(_tickEventMethod);

            _camerasHandler.FreeCam?.Deactivate();
            _camerasHandler.FreeCam = null;

            ModAPI.Sync.TriggerEvent(ToServerEvent.SetInFreecam, false);
        }

        public void ToggleFreecam(Key _ = Key.Noname)
        {
            if (_browserHandler.InInput)
                return;

            _instructionalButtonHandler.Reset();
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
            _eventsHandler.OnFreecamToggled(IsActive);
        }

        #endregion Public Methods

        #region Private Methods

        private void MoveCam()
        {
            var cam = _camerasHandler.FreeCam;

            Position3D pos = cam.Position;
            Position3D dir = cam.Direction;
            Position3D rot = cam.Rotation;

            float rightAxisX = ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptRightAxisX) * 2f;
            float rightAxisY = ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptRightAxisY) * 2f;

            float leftAxisX = ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptLeftAxisX);
            float leftAxisY = ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.ScriptLeftAxisY);

            float slowMult = ModAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Duck) ? 0.5f : 1f;
            float fastMult = ModAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Sprint) ? 3f : 1f;

            if (ModAPI.Control.IsControlJustReleased(InputGroup.MOVE, Control.CursorScrollUp))
                _currentScrollSpeed *= 2f;
            else if (ModAPI.Control.IsControlJustReleased(InputGroup.MOVE, Control.CursorScrollDown))
                _currentScrollSpeed /= 2f;

            Position3D vector = new Position3D
            {
                X = dir.X * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Y = dir.Y * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Z = dir.Z * leftAxisY * slowMult * fastMult * _currentScrollSpeed
            };
            Position3D upVector = new Position3D(0, 0, 1);
            Position3D rightVector = _utilsHandler.GetCrossProduct(dir.Normalized, upVector.Normalized);

            rightVector.X *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Y *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Z *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;

            float goUp = _isUpPressed ? 0.5f : 0f;
            float goDown = _isDownPressed ? 0.5f : 0f;

            Position3D newPos = new Position3D(pos.X - vector.X + rightVector.X, pos.Y - vector.Y + rightVector.Y, pos.Z - vector.Z + rightVector.Z + goUp - goDown);
            if (cam.Position != newPos)
            {
                cam.SetPosition(newPos);
                ModAPI.LocalPlayer.Position = newPos;
            }
            if (ModAPI.Control.IsControlPressed(InputGroup.MOVE, Control.Aim))
            {
                float rotX = Math.Max(Math.Min(rot.X + rightAxisY * -5f, 89), -89);
                var newRot = new Position3D(rotX, 0.0f, rot.Z + rightAxisX * -5f);
                cam.Rotation = newRot;
                ModAPI.LocalPlayer.Rotation = newRot;
            }
        }

        private void OnTick(int _)
        {
            ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_WHEEL);

            if (!_cursorHandler.Visible)
                MoveCam();

            _mapCreatorMarkerHandler.OnTick();
            _mapCreatorObjectPlacingHandler.OnTick();
        }

        #endregion Private Methods
    }
}