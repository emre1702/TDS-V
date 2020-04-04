using System;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Models.GTA;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Utility
{
    public class TDSCamera : ITDSCamera
    {
        public ICam Cam { get; set; }
        public PedBase SpectatingPed { get; set; }
        public bool IsActive => this == ActiveCamera;

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
        public Position3D Direction => Utils.GetDirectionByRotation(Rotation);

        public TDSCamera(IModAPI modAPI)
        {
            Cam = modAPI.Cam.Create();
            modAPI.Event.Tick.Add(new EventMethodData<Action>(OnUpdate, () => SpectatingPed != null));
        }

        ~TDSCamera()
        {
            Cam.Destroy();
        }

        public void OnUpdate()
        {
            var rot = SpectatingPed.GetRotation(2);
            Rotation = rot;
        }

        public void SetPosition(Position3D position, bool instantly = false)
        {
            Position = position;
            if (instantly)
            {
                Cam.Render(true, false, 0);
            }
        }

        public void Spectate(PedBase ped)
        {
            if (SpectatingPed != null)
            {
                Cam.Detach();
            }

            SpectatingPed = ped;
            Cam.AttachCamToPedBone(Handle, ped.Handle, (int)PedBone.SKEL_Head, 0, -2f, 0.3f, true);

            Streaming.SetFocusEntity(ped.Handle);
            FocusAtPos = null;
        }

        public void PointCamAtCoord(float x, float y, float z)
        {
            PointCamAtCoord(new Vector3(x, y, z));
        }

        public void PointCamAtCoord(Vector3 pos)
        {
            Cam.PointCamAtCoord(Handle, pos.X, pos.Y, pos.Z);

            SetFocusArea(pos);
        }

        public void RenderToPosition(float x, float y, float z, bool ease = false, int easeTime = 0)
        {
            SetPosition(x, y, z);
            Render(ease, easeTime);
        }

        public void RenderToPosition(Vector3 pos, bool ease = false, int easeTime = 0)
        {
            RenderToPosition(pos.X, pos.Y, pos.Z, ease, easeTime);
        }

        public static void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = Manager.Lobby.Spectate.SpectatingEntity;
            ActiveCamera?.Deactivate();
            if (spectatingEntity != null)
            {
                Streaming.SetFocusEntity(spectatingEntity.Handle);
                FocusAtPos = null;
                CameraManager.SpectateCam.Activate();
                Cam.RenderScriptCams(true, ease, easeTime, true, false, 0);
            }
            else
            {
                RemoveFocusArea();
                Cam.RenderScriptCams(false, ease, easeTime, true, false, 0);
                ActiveCamera = null;
            }
        }

        public static void SetFocusArea(float x, float y, float z)
        {
            SetFocusArea(new Vector3(x, y, z));
        }

        public static void SetFocusArea(Vector3 pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                Streaming.SetFocusArea(pos.X, pos.Y, pos.Z, 0, 0, 0);
                FocusAtPos = pos;
            }
        }

        public static void RemoveFocusArea()
        {
            Streaming.SetFocusEntity(Player.LocalPlayer.Handle);
            FocusAtPos = null;
        }

        public void Render(bool ease = false, int easeTime = 0)
        {
            Cam.RenderScriptCams(true, ease, easeTime, true, false, 0);
        }

        public void Detach()
        {
            Cam.DetachCam(Handle);
            SpectatingPed = null;
        }

        public void Activate(bool instantly = false)
        {
            Cam.SetCamActive(Handle, true);
            ActiveCamera = this;
            if (instantly)
            {
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
            }

        }

        public void Deactivate(bool instantly = false)
        {
            if (SpectatingPed != null)
                Cam.DetachCam(Handle);
            Cam.SetCamActive(Handle, false);
            ActiveCamera = null;
            if (instantly)
            {
                RemoveFocusArea();
                Cam.RenderScriptCams(false, false, 0, true, false, 0);
            }

        }
    }
}
