using RAGE;
using System;
using System.Collections.Generic;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Deathmatch;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using static RAGE.Events;

namespace TDS.Client.Handler.Appearance.CharCreator
{
    internal class CharCreatorCameraHandler
    {
        private float _currentCamAngle;
        private Position3D _currentCamOffsetPos;
        private Position2D _initMovePedCursorPos;
        private float _initMovingAngle;
        private float _initMovingOffsetZ;
        private TDSTimer _prepareCameraTimer;

        private readonly LoggingHandler _logging;
        private readonly DeathHandler _deathHandler;
        private readonly CharCreatorPedHandler _pedHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly UtilsHandler _utilsHandler;

        public CharCreatorCameraHandler(LoggingHandler loggingHandler, DeathHandler deathHandler, CharCreatorPedHandler pedHandler, CamerasHandler camerasHandler,
            UtilsHandler utilsHandler)
        {
            _logging = loggingHandler;
            _deathHandler = deathHandler;
            _pedHandler = pedHandler;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
        }

        public void Start()
        {
            PrepareCameraDelayed(2000);
            Tick += MovePed;
        }

        public void Stop()
        {
            _prepareCameraTimer?.Kill();
            _prepareCameraTimer = null;
            Tick -= MovePed;
            _currentCamOffsetPos = null;
            _camerasHandler.BetweenRoundsCam.Deactivate(true);
        }

        private void MovePed(List<TickNametagData> _)
        {
            try
            {
                if (_pedHandler.Ped is null)
                    return;
                if (_currentCamOffsetPos is null)
                    return;

                if (!Input.IsDown((int)Key.RightButton))
                {
                    _initMovePedCursorPos = null;
                    return;
                }

                var cursorPos = new Position2D(_utilsHandler.GetCursorX(), _utilsHandler.GetCursorY());
                if (_initMovePedCursorPos is null)
                {
                    _initMovePedCursorPos = cursorPos;
                    _initMovingOffsetZ = _currentCamOffsetPos.Z;
                    _initMovingAngle = _currentCamAngle;
                    return;
                }

                float differenceX = cursorPos.X - _initMovePedCursorPos.X;
                float addToAngle = differenceX * 2 * 360;
                var newAngle = _initMovingAngle + addToAngle;
                while (newAngle < 0)
                    newAngle += 360;
                while (newAngle > 360)
                    newAngle -= 360;

                ApplyAngle(_currentCamOffsetPos, 0.5f, newAngle);

                float differenceY = cursorPos.Y - _initMovePedCursorPos.Y;
                // 0,5 -> 1 => 3
                float addToHeadingY = differenceY * 2 * 3;
                _currentCamOffsetPos.Z = _initMovingOffsetZ + addToHeadingY;

                var cam = _camerasHandler.BetweenRoundsCam;
                cam.LookAt(_pedHandler.Ped, PedBone.SKEL_Head, _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0.15f);
                cam.Render(false, 0);
            }
            catch (Exception ex)
            {
                _logging.LogError(ex);
            }
        }

        public void PrepareCameraDelayed(uint time)
        {
            _prepareCameraTimer?.Kill();
            _prepareCameraTimer = new TDSTimer(PrepareCamera, time);
        }

        private void PrepareCamera()
        {
            try
            {
                _deathHandler.PlayerSpawn();
                _initMovePedCursorPos = null;
                RAGE.Game.Cam.DoScreenFadeIn(200);

                if (_currentCamOffsetPos is null)
                {
                    _currentCamOffsetPos = new Position3D(0, 0, 0.15f);
                    _currentCamAngle = 90;
                    ApplyAngle(_currentCamOffsetPos, 0.5f, _currentCamAngle);
                }

                var cam = _camerasHandler.BetweenRoundsCam;
                cam.LookAt(_pedHandler.Ped, PedBone.SKEL_Head, _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0.15f);

                cam.Activate();
                cam.Render(true, 1000);
                //cam.PointCamAtCoord(new Position3D(-425.48f, 1123.55f, 326.5171f));
                //cam.Activate();
                //cam.RenderToPosition(new Position3D(-425.3048f, 1124.125f, 326.5871f), true, 1000);
            }
            catch (Exception ex)
            {
                _logging.LogError(ex);
            }
        }

        private void ApplyAngle(Position3D pos, float distance, float angle)
        {
            pos.X = (float)Math.Cos(angle * Math.PI / 180) * distance;
            pos.Y = (float)Math.Sin(angle * Math.PI / 180) * distance;
        }
    }
}