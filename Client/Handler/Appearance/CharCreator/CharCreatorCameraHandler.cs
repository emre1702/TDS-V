using RAGE;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Enums.CharCreator;
using TDS.Client.Handler.Deathmatch;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models.GTA;
using static RAGE.Events;
using static RAGE.Ui.Cursor;

namespace TDS.Client.Handler.Appearance.CharCreator
{
    internal class CharCreatorCameraHandler
    {
        private float _currentCamAngle;
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
            RAGE.Events.Add(FromBrowserEvent.CharCreatorResetCameraTarget, (_) => SetCameraTarget(CharCreatorCameraTarget.Head));
        }

        public void Start()
        {
            _currentTarget = CharCreatorCameraTarget.Head;
            PrepareCameraDelayed(1000);
            Tick += MovePed;
        }

        public void Stop()
        {
            _prepareCameraTimer?.Kill();
            _prepareCameraTimer = null;
            Tick -= MovePed;
            _camerasHandler.BetweenRoundsCam.Deactivate(true);
        }

        public void SetCameraTarget(CharCreatorCameraTarget target)
        {
            if (_currentTarget == target)
                return;
            _currentTarget = target;

            _initMovePedCursorPos = null;
            _isCameraSwitchingTargetTimer?.Kill();
            _isCameraSwitchingTarget = true;
            _isCameraSwitchingTargetTimer = new TDSTimer(() =>
            {
                _isCameraSwitchingTarget = false;
                _isCameraSwitchingTargetTimer = null;
            }, 1000);

            var offsetPos = GetOffsetPos(2f, 90);
            LookAtTarget(offsetPos);
            _camerasHandler.BetweenRoundsCam.Render(true, 1000);
        }

        private void MovePed(List<TickNametagData> _)
        {
            try
            {
                if (_pedHandler.Ped is null)
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
                    _initMovingOffsetZ = 0;
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

                var offsetPos = GetOffsetPos(2f, _currentCamAngle);

                float differenceY = cursorPos.Y - _initMovePedCursorPos.Y;
                // 0,5 -> 1 => 3
                float addToHeadingY = differenceY * 2 * 3;
                offsetPos.Z = _initMovingOffsetZ + addToHeadingY;

                LookAtTarget(offsetPos);
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

                var offsetPos = GetOffsetPos(2f, 90);
                LookAtTarget(offsetPos);
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

        private Position3D GetOffsetPos(float distance, float angle)
        {
            var radiansAngle = (Math.PI / 180) * angle;
            var offsetX = (float)Math.Cos(radiansAngle) * distance;
            var offsetY = (float)Math.Sin(radiansAngle) * distance;
            return new Position3D(offsetX, offsetY, 0);
        }

        private void LookAtTarget(Position3D offsetPos)
        {
            switch (_currentTarget)
            {
                case CharCreatorCameraTarget.All: LookAtAll(offsetPos); break;
                case CharCreatorCameraTarget.Head: LookAtHead(offsetPos); break;
                case CharCreatorCameraTarget.Torso: LookAtTorso(offsetPos); break;
                case CharCreatorCameraTarget.Arms: LookAtArms(offsetPos); break;
                case CharCreatorCameraTarget.Hands: LookAtHands(offsetPos); break;
                case CharCreatorCameraTarget.Legs: LookAtLegs(offsetPos); break;
                case CharCreatorCameraTarget.Foot: LookAtFoot(offsetPos); break;
            }
        }

        private void LookAtAll(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_ROOT,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtHead(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_Head,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtTorso(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_ROOT,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtArms(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.RB_R_ArmRoll,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtHands(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Hand,
               offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtLegs(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Calf,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }

        private void LookAtFoot(Position3D offsetPos)
        {
            _camerasHandler.BetweenRoundsCam.LookAt(_pedHandler.Ped, PedBone.SKEL_R_Foot,
                offsetPos.X, offsetPos.Y, offsetPos.Z);
        }
    }
}