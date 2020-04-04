using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Cam
{
    class Cam : ICam
    {
        public Position3D Position
        {
            get => RAGE.Game.Cam.GetCamCoord(_handle).ToPosition3D();
            set => RAGE.Game.Cam.SetCamCoord(_handle, value.X, value.Y, value.Z);
        }

        public Position3D Rotation
        {
            get => RAGE.Game.Cam.GetCamRot(_handle, 2).ToPosition3D();
            set => RAGE.Game.Cam.SetCamRot(_handle, value.X, value.Y, value.Z, 2);
        }

        private readonly int _handle;

        public Cam(int handle)
            => _handle = handle;

        public void Render(bool render, bool ease, int easeTime)
        {
            RAGE.Game.Cam.RenderScriptCams(render, ease, easeTime, true, false, 0);
        }

        public void Detach()
        {
            RAGE.Game.Cam.DetachCam(_handle);
        }

        public void Destroy()
        {
            RAGE.Game.Cam.DestroyCam(_handle, false);
        }
    }
}
