using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Cam
{
    internal class Cam : ICam
    {
        #region Private Fields

        private readonly int _handle;

        #endregion Private Fields

        #region Public Constructors

        public Cam(int handle)
            => _handle = handle;

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        public void AttachTo(IPedBase ped, PedBone bone, int x, float y, float z, bool heading)
        {
            RAGE.Game.Cam.AttachCamToPedBone(_handle, ped.Handle, (int)bone, x, y, z, heading);
        }

        public void AttachTo(IEntityBase ped, PedBone bone, float x, float y, float z, bool heading)
        {
            RAGE.Game.Cam.AttachCamToPedBone(_handle, ped.Handle, (int)bone, x, y, z, heading);
        }

        public void Destroy()
        {
            RAGE.Game.Cam.DestroyCam(_handle, false);
        }

        public void Detach()
        {
            RAGE.Game.Cam.DetachCam(_handle);
        }

        public void PointAtCoord(Position3D pos)
        {
            RAGE.Game.Cam.PointCamAtCoord(_handle, pos.X, pos.Y, pos.Z);
        }

        public void Render(bool render, bool ease, int easeTime)
        {
            RAGE.Game.Cam.RenderScriptCams(render, ease, easeTime, true, false, 0);
        }

        public void SetActive(bool active)
        {
            RAGE.Game.Cam.SetCamActive(_handle, active);
        }

        public void SetFov(float fov)
        {
            RAGE.Game.Cam.SetCamFov(_handle, fov);
        }

        #endregion Public Methods
    }
}
