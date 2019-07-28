using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    class FreeCam
    {
        private static bool _inFreeCam;
        private static FreeCamMoveDto _moveDto;

        static FreeCam()
        {
            TickManager.Add(Render, () => _inFreeCam && !ChatManager.IsOpen && !Cursor.Visible);
        }

        public static void Render()
        {
            /* 
             * API.disableControlThisFrame( 16 );
		    API.disableControlThisFrame( 17 );
		    API.disableControlThisFrame( 26 );
		    API.disableControlThisFrame( 24 ); */

            /*var cam = CameraManager.FreeCam;
            var camPos = cam.Position;
            Control.
            var camRot = cam.Rotation;
            float speed = GetCamSpeed();
            int direction =

            var addPos1 = new Vector3(
                camRot.X * speed * (;
            );

            cam.SetPosition(
                camPos.X - addPos1.X + addPos2.X,
                camPos.Y - addPos1.Y + addPos2.Y,
                camPos.Z - addPos1.Z + addPos2.Z + upMovement - downMovement,
            );*/
            
        }

        private static float GetCamSpeed()
        {
            float speed = 1;
            if (_moveDto.Boost)
            {
                speed += 2 + Math.Min((TimerManager.ElapsedTicks - _moveDto.BoostStartTick) / 12000 * 7, 7);
            }
            else if (_moveDto.Slow)
            {
                speed = 0.3f;
            }
            return speed;
        }

        public static void FreeCamControlDown(Control control)
        {
            switch (control)
            {
                case Control.MoveUp:
                    _moveDto.Up = true;
                    _moveDto.Down = false;
                    break;

                case Control.MoveDown:
                    _moveDto.Down = true;
                    _moveDto.Up = false;
                    break;

                case Control.MoveLeft:
                    _moveDto.Left = true;
                    _moveDto.Right = false;
                    break;

                case Control.MoveRight:
                    _moveDto.Right = true;
                    _moveDto.Left = false;
                    break;

                case Control.LookUp:
                    _moveDto.LookUp = true;
                    _moveDto.LookDown = false;
                    break;

                case Control.LookDown:
                    _moveDto.LookDown = true;
                    _moveDto.LookUp = false;
                    break;

                case Control.LookLeft:
                    _moveDto.LookLeft = true;
                    _moveDto.LookRight = false;
                    break;

                case Control.LookRight:
                    _moveDto.LookRight = true;
                    _moveDto.LookLeft = false;
                    break;

                case Control.Sprint:
                    _moveDto.BoostStartTick = TimerManager.ElapsedTicks;
                    _moveDto.Boost = true;
                    _moveDto.Slow = false;
                    break;

                case Control.Duck:
                    _moveDto.Slow = true;
                    _moveDto.Boost = false;
                    break;
            }
        }

        public static void FreeCamControlUp(Control control)
        {
            switch (control)
            {
                case Control.MoveUp:
                    _moveDto.Up = false;
                    break;

                case Control.MoveDown:
                    _moveDto.Down = false;
                    break;

                case Control.MoveLeft:
                    _moveDto.Left = false;
                    break;

                case Control.MoveRight:
                    _moveDto.Right = false;
                    break;

                case Control.LookUp:
                    _moveDto.LookUp = false;
                    break;

                case Control.LookDown:
                    _moveDto.LookDown = false;
                    break;

                case Control.LookLeft:
                    _moveDto.LookLeft = false;
                    break;

                case Control.LookRight:
                    _moveDto.LookRight = false;
                    break;

                case Control.Sprint:
                    _moveDto.Boost = false;
                    break;

                case Control.Duck:
                    _moveDto.Slow = false;
                    break;
            }
        }

        private static void Bind()
        {
            BindManager.Add(Control.MoveUp, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.MoveDown, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.MoveLeft, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.MoveRight, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Add(Control.LookUp, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.LookDown, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.LookLeft, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.LookRight, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Add(Control.Sprint, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Add(Control.Duck, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Add(Control.MoveUp, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.MoveDown, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.MoveLeft, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.MoveRight, FreeCamControlUp, EKeyPressState.Up);

            BindManager.Add(Control.LookUp, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.LookDown, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.LookLeft, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.LookRight, FreeCamControlUp, EKeyPressState.Up);

            BindManager.Add(Control.Sprint, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Add(Control.Duck, FreeCamControlUp, EKeyPressState.Up);
        }

        private static void Unbind()
        {
            BindManager.Remove(Control.MoveUp, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.MoveDown, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.MoveLeft, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.MoveRight, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Remove(Control.LookUp, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.LookDown, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.LookLeft, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.LookRight, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Remove(Control.Sprint, FreeCamControlDown, EKeyPressState.Down);
            BindManager.Remove(Control.Duck, FreeCamControlDown, EKeyPressState.Down);

            BindManager.Remove(Control.MoveUp, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.MoveDown, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.MoveLeft, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.MoveRight, FreeCamControlUp, EKeyPressState.Up);

            BindManager.Remove(Control.LookUp, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.LookDown, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.LookLeft, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.LookRight, FreeCamControlUp, EKeyPressState.Up);

            BindManager.Remove(Control.Sprint, FreeCamControlUp, EKeyPressState.Up);
            BindManager.Remove(Control.Duck, FreeCamControlUp, EKeyPressState.Up);
        }

        public static void Start()
        {
            if (_inFreeCam) 
                return;
            _inFreeCam = true;
            _moveDto = new FreeCamMoveDto();

            var cam = CameraManager.FreeCam;
            cam.Activate();
            cam.SetPosition(Player.LocalPlayer.Position, true);
            Ui.DisplayRadar(false);
            Player.LocalPlayer.FreezePosition(true);
            Bind();
        }

        public static void Stop()
        {
            if (!_inFreeCam)
                return;
            _inFreeCam = true;

            CameraManager.FreeCam.Deactivate(true);
            Ui.DisplayRadar(true);
            Player.LocalPlayer.FreezePosition(false);
            Unbind();
        }

        private class FreeCamMoveDto
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;

            public bool LookUp;
            public bool LookDown;
            public bool LookLeft;
            public bool LookRight;

            public bool Boost;
            public bool Slow;

            public ulong BoostStartTick;
        }
    }
}
