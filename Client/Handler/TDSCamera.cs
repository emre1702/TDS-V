using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System.Collections.Generic;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums;
using static RAGE.Events;

namespace TDS.Client.Handler.Entities
{
    public class TDSCamera
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly int _handle;
        private readonly LoggingHandler _loggingHandler;

        private readonly UtilsHandler _utilsHandler;
        private GameEntityBase _spectatingEntity;

        public TDSCamera(string name, LoggingHandler loggingHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler)
        {
            _loggingHandler = loggingHandler;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;

            Name = name;
            _handle = Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
        }

        ~TDSCamera() => Destroy();

        public Vector3 Direction => _utilsHandler.GetDirectionByRotation(Rotation);
        public bool IsActive => this == _camerasHandler.ActiveCamera;
        public string Name { get; set; }

        public Vector3 Position
        {
            get => Cam.GetCamCoord(_handle);
            set => SetPosition(value);
        }

        public Vector3 Rotation
        {
            get => Cam.GetCamRot(_handle, 2);
            set => Cam.SetCamRot(_handle, value.X, value.Y, value.Z, 2);
        }

        public GameEntityBase SpectatingEntity
        {
            get => _spectatingEntity;
            private set
            {
                _spectatingEntity = value;
                if (value is null)
                    RAGE.Events.Tick -= OnUpdate;
                else
                    RAGE.Events.Tick += OnUpdate;
            }
        }

        public void Activate(bool instantly = false)
        {
            if (_camerasHandler.ActiveCamera == this)
                return;
            if (!(_camerasHandler.ActiveCamera is null))
                _camerasHandler.ActiveCamera.Deactivate();
            Cam.SetCamActive(_handle, true);
            _camerasHandler.ActiveCamera = this;
            if (instantly)
            {
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
            }
        }

        public void Attach(PedBase ped, PedBone bone, int x, float y, float z, bool heading)
        {
            _loggingHandler.LogInfo("Attach at " + (ped is ITDSPlayer player ? player.Name : "Ped"), "TDSCamera.Attach " + Name);
            Cam.AttachCamToPedBone(_handle, ped.Handle, (int)bone, x, y, z, heading);
        }

        public void Deactivate(bool instantly = false)
        {
            if (SpectatingEntity != null)
                Cam.DetachCam(_handle);
            Cam.SetCamActive(_handle, false);
            _camerasHandler.ActiveCamera = null;
            if (instantly)
            {
                _camerasHandler.RemoveFocusArea();
                Cam.RenderScriptCams(false, false, 0, true, false, 0);
            }
        }

        public void Destroy()
        {
            Cam.DestroyCam(_handle, false);
        }

        public void Detach()
        {
            Cam.DetachCam(_handle);
            SpectatingEntity = null;
        }

        public void LookAt(PedBase ped, PedBone bone, float posOffsetX, float posOffsetY, float posOffsetZ,
            float lookAtOffsetX, float lookAtOffsetZ)
        {
            var pos = ped.GetBoneCoords((int)bone, posOffsetZ, posOffsetY, posOffsetX);
            Cam.SetCamCoord(_handle, pos.X, pos.Y, pos.Z);
            var pointAt = ped.GetBoneCoords((int)bone, lookAtOffsetX, 0, lookAtOffsetZ);
            Cam.PointCamAtCoord(_handle, pointAt.X, pointAt.Y, pointAt.Z);

            Streaming.SetFocusEntity(ped.Handle);
            _camerasHandler.FocusAtPos = null;
        }

        public void OnUpdate(List<TickNametagData> _)
        {
            if (!(SpectatingEntity is null))
                Rotation = SpectatingEntity.GetRotation(2);
        }

        public void PointCamAtCoord(Vector3 pos)
        {
            _loggingHandler.LogInfo("", "TDSCamera.PointCamAtCoord " + Name);
            Cam.PointCamAtCoord(_handle, pos.X, pos.Y, pos.Z);
            _camerasHandler.SetFocusArea(pos);
        }

        public void Render(bool ease = false, int easeTime = 0)
        {
            Cam.RenderScriptCams(true, ease, easeTime, true, false, 0);
        }

        public void RenderToPosition(Vector3 pos, bool ease = false, int easeTime = 0)
        {
            _loggingHandler.LogInfo("", "TDSCamera.RenderToPosition " + Name);
            SetPosition(pos);
            Render(ease, easeTime);
            _loggingHandler.LogInfo("", "TDSCamera.RenderToPosition " + Name, true);
        }

        public void SetFov(float fov)
        {
            Cam.SetCamFov(_handle, fov);
        }

        public void SetPosition(Vector3 position, bool instantly = false)
        {
            _loggingHandler.LogInfo("", "TDSCamera.SetPosition " + Name);
            Cam.SetCamCoord(_handle, position.X, position.Y, position.Z);

            if (instantly)
            {
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
            }
            _loggingHandler.LogInfo("", "TDSCamera.SetPosition " + Name, true);
        }

        public void Spectate(GameEntityBase entity)
        {
            //Todo
            if (entity is PedBase ped)
                Spectate(ped);
        }

        public void Spectate(PedBase ped)
        {
            _loggingHandler.LogInfo("Spectate " + (ped is ITDSPlayer player ? player.Name : "Ped"), "TDSCamera.Spectate " + Name);
            if (SpectatingEntity != null)
            {
                Cam.DetachCam(_handle);
            }

            SpectatingEntity = ped;
            Cam.AttachCamToPedBone(_handle, ped.Handle, (int)PedBone.SKEL_Head, 0, -2f, 0.3f, true);

            RAGE.Game.Streaming.SetFocusEntity(ped.Handle);
            _camerasHandler.FocusAtPos = null;
        }
    }
}
