using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFreecamHandler : ServiceBase
    {
        public bool IsActive;

        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly MapCreatorFootHandler _mapCreatorFootHandler;
        private readonly MapCreatorMarkerHandler _mapCreatorMarkerHandler;
        private readonly MapCreatorObjectPlacingHandler _mapCreatorObjectPlacingHandler;
        private readonly UtilsHandler _utilsHandler;
        private float _currentScrollSpeed = 1f;
        private bool _isDownPressed;
        private bool _isUpPressed;

        public MapCreatorFreecamHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler,
            InstructionalButtonHandler instructionalButtonHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, MapCreatorFootHandler mapCreatorFootHandler,
            MapCreatorMarkerHandler mapCreatorMarkerHandler, MapCreatorObjectPlacingHandler mapCreatorObjectPlacingHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
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

        public void Start()
        {
            IsActive = true;
            Tick += OnTick;

            if (_camerasHandler.FreeCam is null)
                _camerasHandler.FreeCam = new TDSCamera("FreeCam", Logging, _camerasHandler, _utilsHandler);

            var player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(true);
            player.SetVisible(false, false);
            player.SetCollision(false, false);

            var cam = _camerasHandler.FreeCam;
            cam.Position = RAGE.Game.Cam.GetGameplayCamCoord();
            cam.Rotation = RAGE.Game.Cam.GetGameplayCamRot(2);
            cam.SetFov(RAGE.Game.Cam.GetGameplayCamFov());

            cam.Activate();
            cam.Render();

            RAGE.Events.CallRemote(ToServerEvent.SetInFreecam, true);
        }

        public void Stop()
        {
            IsActive = false;
            Tick -= OnTick;

            _camerasHandler.FreeCam?.Deactivate();
            _camerasHandler.FreeCam = null;

            RAGE.Events.CallRemote(ToServerEvent.SetInFreecam, false);
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

        private void MoveCam()
        {
            var cam = _camerasHandler.FreeCam;

            var pos = cam.Position;
            var dir = cam.Direction;
            var rot = cam.Rotation;

            float rightAxisX = RAGE.Game.Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.ScriptRightAxisX) * 2f;
            float rightAxisY = RAGE.Game.Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.ScriptRightAxisY) * 2f;

            float leftAxisX = RAGE.Game.Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.ScriptLeftAxisX);
            float leftAxisY = RAGE.Game.Pad.GetDisabledControlNormal((int)InputGroup.MOVE, (int)Control.ScriptLeftAxisY);

            float slowMult = RAGE.Game.Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Duck) ? 0.5f : 1f;
            float fastMult = RAGE.Game.Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Sprint) ? 3f : 1f;

            if (RAGE.Game.Pad.IsControlJustReleased((int)InputGroup.MOVE, (int)Control.CursorScrollUp))
                _currentScrollSpeed *= 2f;
            else if (RAGE.Game.Pad.IsControlJustReleased((int)InputGroup.MOVE, (int)Control.CursorScrollDown))
                _currentScrollSpeed /= 2f;

            var vector = new Vector3
            {
                X = dir.X * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Y = dir.Y * leftAxisY * slowMult * fastMult * _currentScrollSpeed,
                Z = dir.Z * leftAxisY * slowMult * fastMult * _currentScrollSpeed
            };
            var upVector = new Vector3(0, 0, 1);
            var rightVector = _utilsHandler.GetCrossProduct(dir.Normalized, upVector.Normalized);

            rightVector.X *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Y *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;
            rightVector.Z *= leftAxisX * 0.5f * slowMult * fastMult * _currentScrollSpeed;

            float goUp = _isUpPressed ? 0.5f : 0f;
            float goDown = _isDownPressed ? 0.5f : 0f;

            var newPos = new Vector3(pos.X - vector.X + rightVector.X, pos.Y - vector.Y + rightVector.Y, pos.Z - vector.Z + rightVector.Z + goUp - goDown);
            if (cam.Position != newPos)
            {
                cam.SetPosition(newPos);
                RAGE.Elements.Player.LocalPlayer.Position = newPos;
            }
            if (RAGE.Game.Pad.IsControlPressed((int)InputGroup.MOVE, (int)Control.Aim))
            {
                float rotX = Math.Max(Math.Min(rot.X + rightAxisY * -5f, 89), -89);
                var newRot = new Vector3(rotX, 0.0f, rot.Z + rightAxisX * -5f);
                cam.Rotation = newRot;
                (RAGE.Elements.Player.LocalPlayer as ITDSPlayer).Rotation = newRot;
            }
        }

        private void OnTick(List<TickNametagData> _)
        {
            RAGE.Game.Ui.HideHudComponentThisFrame((int)HudComponent.WeaponWheel);

            if (!_cursorHandler.Visible)
                MoveCam();

            _mapCreatorMarkerHandler.OnTick();
            _mapCreatorObjectPlacingHandler.OnTick();
        }
    }
}
