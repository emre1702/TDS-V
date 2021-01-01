using RAGE;
using System;
using System.Collections.Generic;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Enums.CharCreator;
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
        private CharCreatorCameraTarget _currentTarget;
        private bool _isCameraSwitchingTarget;
        private TDSTimer _isCameraSwitchingTargetTimer;
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

            //Todo: Call this in main menu OnInit
            RAGE.Events.Add(FromBrowserEvent.CharCreatorResetCameraTarget, (_) => SetCameraTarget(CharCreatorCameraTarget.All));
        }

        public void Start()
        {
            _currentTarget = CharCreatorCameraTarget.All;
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

        public void SetCameraTarget(CharCreatorCameraTarget target)
        {
            if (_currentTarget == target)
                return;
            _currentTarget = target;
            _isCameraSwitchingTargetTimer?.Kill();
            _isCameraSwitchingTarget = true;
            _isCameraSwitchingTargetTimer = new TDSTimer(() =>
            {
                _isCameraSwitchingTarget = false;
                _isCameraSwitchingTargetTimer = null;
            }, 1000);
            _camerasHandler.BetweenRoundsCam.Render(true, 1000);
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

                var cursorPos = _utilsHandler.GetCursorPos();
                if (_initMovePedCursorPos is null)
                {
                    _initMovePedCursorPos = cursorPos;
                    _initMovingOffsetZ = _currentCamOffsetPos.Z;
                    _initMovingAngle = _currentCamAngle;
                    return;
                }

                float differenceX = cursorPos.X - _initMovePedCursorPos.X;
                float addToAngle = differenceX * 2 * 360;
                _currentCamAngle = _initMovingAngle + addToAngle;
                while (_currentCamAngle < 0)
                    _currentCamAngle += 360;
                while (_currentCamAngle > 360)
                    _currentCamAngle -= 360;

                ApplyAngle(_currentCamOffsetPos, 2f, _currentCamAngle);

                float differenceY = cursorPos.Y - _initMovePedCursorPos.Y;
                // 0,5 -> 1 => 3
                //float addToHeadingY = differenceY * 2 * 3;
                //_currentCamOffsetPos.Z = _initMovingOffsetZ + addToHeadingY;

                LookAtTarget();
                if (!_isCameraSwitchingTarget)
                    _camerasHandler.BetweenRoundsCam.Render(false, 0);
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

                LookAtTarget();
                var cam = _camerasHandler.BetweenRoundsCam;
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
            var radiansAngle = (Math.PI / 180) * angle;
            pos.X = (float)Math.Cos(radiansAngle) * distance;
            pos.Y = (float)Math.Sin(radiansAngle) * distance;
        }

        private void LookAtTarget()
        {
            if (_currentCamOffsetPos is null)
            {
                _currentCamOffsetPos = new Position3D(0, 0, 0.15);
                _currentCamAngle = 90;
                ApplyAngle(_currentCamOffsetPos, 2f, _currentCamAngle);
            }

            switch (_currentTarget)
            {
                case CharCreatorCameraTarget.All: LookAtAll(); break;
                case CharCreatorCameraTarget.Head: LookAtHead(); break;
                case CharCreatorCameraTarget.Torso: LookAtTorso(); break;
                case CharCreatorCameraTarget.Arms: LookAtArms(); break;
                case CharCreatorCameraTarget.Hands: LookAtHands(); break;
                case CharCreatorCameraTarget.Legs: LookAtLegs(); break;
                case CharCreatorCameraTarget.Foot: LookAtFoot(); break;
            }
        }

        private void LookAtAll()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_ROOT,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtHead()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_Head,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtTorso()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_ROOT,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtArms()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.RB_R_ArmRoll,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtHands()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Hand,
               _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtLegs()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Calf,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }

        private void LookAtFoot()
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Foot,
                _currentCamOffsetPos.X, _currentCamOffsetPos.Y, _currentCamOffsetPos.Z, 0, 0);
        }
    }
}