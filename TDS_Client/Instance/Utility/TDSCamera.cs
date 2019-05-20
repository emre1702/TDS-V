using System;
using RAGE;
using RAGE.Game;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Utility
{
    class TDSCamera
    {
        public int Handle { get; set; }

        public Vector3 Position
        {
            get => Cam.GetCamCoord(Handle);
            set => SetPosition(value);
        }
        public Vector3 Rotation
        {
            get => Cam.GetCamRot(Handle, 2);
            set => Cam.SetCamRot(Handle, value.X, value.Y, value.Z, 2);
        }

        public TDSCamera()
        {
            Handle = Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
        }

        public void SetPosition(Vector3 position, bool instantly = false)
        {
            SetPosition(position.X, position.Y, position.Z, instantly);
        }

        public void SetPosition(float x, float y, float z, bool instantly = false)
        {
            Cam.SetCamCoord(Handle, x, y, z);
            if (instantly)
            {
                Streaming.SetFocusEntity(Handle);
                Cam.RenderScriptCams(true, false, 0, true, true, 0);
            }
                
        }

        public void PointCamAtCoord(float x, float y, float z)
        {
            Cam.PointCamAtCoord(Handle, x, y, z);
        }

        public void RenderToPosition(float x, float y, float z, bool ease = false, int easeTime = 0)
        {
            SetPosition(x, y, z);
            Render(ease, easeTime);
        }

        public static void RenderBack(bool ease = false, int easeTime = 0)
        {
            Streaming.SetFocusEntity(Player.LocalPlayer.Handle);
            Cam.RenderScriptCams(false, ease, easeTime, true, true, 0);
        }

        public void Render(bool ease = false, int easeTime = 0)
        {
            Streaming.SetFocusEntity(Handle);
            Cam.RenderScriptCams(true, ease, easeTime, true, true, 0);
        }

        public void Activate(bool instantly = false)
        {
            Cam.SetCamActive(Handle, true);
            if (instantly)
            {
                Streaming.SetFocusEntity(Handle);
                Cam.RenderScriptCams(true, false, 0, true, true, 0);
            }
                
        }

        public void Deactivate(bool instantly = false)
        {
            Cam.SetCamActive(Handle, false);
             if (instantly)
            {
                Streaming.SetFocusEntity(Player.LocalPlayer.Handle);
                Cam.RenderScriptCams(false, false, 0, true, true, 0);
            }
                
        }
    }
}
