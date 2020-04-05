using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Models;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Instance.Utility
{
    public class TDSCamera : ITDSCamera
    {
        public ICam Cam { get; set; }
        public IPedBase SpectatingPed { get; set; }
        public bool IsActive => this == _camerasHandler.ActiveCamera;

        public static Position3D FocusAtPos { get; set; }

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
            modAPI.Event.Tick.Add(new EventMethodData<Action>(OnUpdate, () => SpectatingPed != null));
        }

        ~TDSCamera()
        {
            Cam.Destroy();
        }

        public void OnUpdate()
        {
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
            FocusAtPos = null;
        }

        public void PointCamAtCoord(Position3D pos)
        {
            Cam.PointAtCoord(pos);

            SetFocusArea(pos);
        }

        public void RenderToPosition(Position3D pos, bool ease = false, int easeTime = 0)
        {
            SetPosition(pos);
            Render(ease, easeTime);
        }

        public void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = _spectatingHandler.SpectatingEntity;
            _camerasHandler.ActiveCamera?.Deactivate();
            if (spectatingEntity != null)
            {
                _modAPI.Streaming.SetFocusEntity(spectatingEntity.Handle);
                FocusAtPos = null;
                _camerasHandler.SpectateCam.Activate();
                Cam.Render(true, ease, easeTime);
            }
            else
            {
                RemoveFocusArea();
                Cam.Render(false, ease, easeTime);
                _camerasHandler.ActiveCamera = null;
            }
        }

        public void SetFocusArea(Position3D pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                _modAPI.Streaming.SetFocusArea(pos, 0, 0, 0);
                FocusAtPos = pos;
            }
        }

        public void RemoveFocusArea()
        {
            _modAPI.Streaming.SetFocusEntity(_modAPI.LocalPlayer);
            FocusAtPos = null;
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
                RemoveFocusArea();
                Cam.Render(false, false, 0);
            }

        }
    }
}
