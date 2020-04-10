using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Models;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Entities
{
    public class TDSCamera
    {
        public ICam Cam { get; set; }
        public IPedBase SpectatingPed { get; set; }
        public bool IsActive => this == _camerasHandler.ActiveCamera;

        public Position3D Position
        {
            get => Cam.Position;
            set => SetPosition(value);
        }
        public Position3D Rotation
        {
            get => Cam.Rotation;
            set => Cam.Rotation = value;
        }
        public Position3D Direction => _utilsHandler.GetDirectionByRotation(Rotation);

        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly SpectatingHandler _spectatingHandler;

        public TDSCamera(IModAPI modAPI, CamerasHandler camerasHandler, UtilsHandler utilsHandler, SpectatingHandler spectatingHandler)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;
            _spectatingHandler = spectatingHandler;

            Cam = modAPI.Cam.Create();
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnUpdate, () => SpectatingPed != null));
        }

        ~TDSCamera()
        {
            Cam.Destroy();
        }

        public void SetFov(float fov)
        {
            Cam.SetFov(fov);
        }

        public void OnUpdate(ulong currentMs)
        {
            if (!(SpectatingPed is null))
                Rotation = SpectatingPed.Rotation;
        }

        public void SetPosition(Position3D position, bool instantly = false)
        {
            Position = position;
            if (instantly)
            {
                Cam.Render(true, false, 0);
            }
        }

        public void Spectate(IPedBase ped)
        {
            if (SpectatingPed != null)
            {
                Cam.Detach();
            }

            SpectatingPed = ped;
            Cam.AttachTo(ped, PedBone.SKEL_Head, 0, -2f, 0.3f, true);

            _modAPI.Streaming.SetFocusEntity(ped);
            _camerasHandler.FocusAtPos = null;
        }

        public void PointCamAtCoord(Position3D pos)
        {
            Cam.PointAtCoord(pos);

            _camerasHandler.SetFocusArea(pos);
        }

        public void RenderToPosition(Position3D pos, bool ease = false, int easeTime = 0)
        {
            SetPosition(pos);
            Render(ease, easeTime);
        }

        public void Render(bool ease = false, int easeTime = 0)
        {
            Cam.Render(true, ease, easeTime);
        }

        public void Detach()
        {
            Cam.Detach();
            SpectatingPed = null;
        }

        public void Activate(bool instantly = false)
        {
            Cam.SetActive(true);
            _camerasHandler.ActiveCamera = this;
            if (instantly)
            {
                Cam.Render(true, false, 0);
            }

        }

        public void Deactivate(bool instantly = false)
        {
            if (SpectatingPed != null)
                Cam.Detach();
            Cam.SetActive(false);
            _camerasHandler.ActiveCamera = null;
            if (instantly)
            {
                _camerasHandler.RemoveFocusArea();
                Cam.Render(false, false, 0);
            }

        }
    }
}
