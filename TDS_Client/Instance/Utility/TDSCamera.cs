using RAGE;
using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Manager.Utility;
using TDS_Shared.Enum;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Utility
{
    class TDSCamera
    {
        public int Handle { get; set; }
        public PedBase SpectatingPed { get; set; }
        public bool IsActive => this == ActiveCamera;

        public static TDSCamera ActiveCamera { get; set; }
        public static Vector3 FocusAtPos { get; set; }

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
        public Vector3 Direction => ClientUtils.GetDirectionByRotation(Rotation);

        public TDSCamera()
        {
            Handle = Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
            TickManager.Add(OnUpdate, () => SpectatingPed != null);
        }

        ~TDSCamera()
        {
            Cam.DestroyCam(Handle, false);
        }

        public void OnUpdate()
        {
            var rot = SpectatingPed.GetRotation(2);
            Cam.SetCamRot(Handle, rot.X, rot.Y, rot.Z, 2);
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
                Cam.RenderScriptCams(true, false, 0, true, false, 0);
            }
        }

        public void Spectate(PedBase ped)
        {
            if (SpectatingPed != null)
            {
                Cam.DetachCam(Handle);
            }
                
            SpectatingPed = ped;
            Cam.AttachCamToPedBone(Handle, ped.Handle, (int) PedBone.SKEL_Head, 0, -2f, 0.3f, true);

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
