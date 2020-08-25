using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Cam;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Entities
{
    public class TDSCamera
    {
        #region Private Fields

        private readonly CamerasHandler _camerasHandler;
        private readonly LoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;

        #endregion Private Fields

        #region Public Constructors

        public TDSCamera(string name, IModAPI modAPI, LoggingHandler loggingHandler, CamerasHandler camerasHandler, UtilsHandler utilsHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _camerasHandler = camerasHandler;
            _utilsHandler = utilsHandler;

            Name = name;
            Cam = modAPI.Cam.Create();
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnUpdate, () => SpectatingEntity != null));
        }

        #endregion Public Constructors


        #region Public Properties

        public ICam Cam { get; set; }
        public Position Direction => _utilsHandler.GetDirectionByRotation(Rotation);
        public bool IsActive => this == _camerasHandler.ActiveCamera;

        public Position Position
        {
            get => Cam.Position;
            set => SetPosition(value);
        }

        public Position Rotation
        {
            get => Cam.Rotation;
            set => Cam.Rotation = value;
        }

        public IEntityBase SpectatingEntity { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Activate(bool instantly = false)
        {
            if (_camerasHandler.ActiveCamera == this)
                return;
            if (!(_camerasHandler.ActiveCamera is null))
                _camerasHandler.ActiveCamera.Deactivate();
            Cam.SetActive(true);
            _camerasHandler.ActiveCamera = this;
            if (instantly)
            {
                Cam.Render(true, false, 0);
            }
        }

        public void Attach(IEntityBase ped, PedBone bone, int x, float y, float z, bool heading)
        {
            Cam.AttachTo(ped, bone, x, y, z, heading);
        }

        public void Deactivate(bool instantly = false)
        {
            if (SpectatingEntity != null)
                Cam.Detach();
            Cam.SetActive(false);
            _camerasHandler.ActiveCamera = null;
            if (instantly)
            {
                _camerasHandler.RemoveFocusArea();
                Cam.Render(false, false, 0);
            }
        }

        public void Detach()
        {
            Cam.Detach();
            SpectatingEntity = null;
        }

        public void LookAt(IPedBase ped, PedBone bone, float posOffsetX, float posOffsetY, float posOffsetZ,
            float lookAtOffsetX, float lookAtOffsetZ)
        {
            Cam.Position = ped.GetBoneCoords(bone, posOffsetZ, posOffsetY, posOffsetX);
            Cam.PointAtCoord(ped.GetBoneCoords(bone, lookAtOffsetX, 0, lookAtOffsetZ));

            _modAPI.Streaming.SetFocusEntity(ped);
            _camerasHandler.FocusAtPos = null;
        }

        public void OnUpdate(int currentMs)
        {
            if (!(SpectatingEntity is null))
                Rotation = SpectatingEntity.Rotation;
        }

        public void PointCamAtCoord(Position pos)
        {
            Cam.PointAtCoord(pos);

            _camerasHandler.SetFocusArea(pos);
        }

        public void Render(bool ease = false, int easeTime = 0)
        {
            Cam.Render(true, ease, easeTime);
        }

        public void RenderToPosition(Position pos, bool ease = false, int easeTime = 0)
        {
            _loggingHandler.LogInfo("", "TDSCamera.RenderToPosition");
            SetPosition(pos);
            Render(ease, easeTime);
            _loggingHandler.LogInfo("", "TDSCamera.RenderToPosition", true);
        }

        public void SetFov(float fov)
        {
            Cam.SetFov(fov);
        }

        public void SetPosition(Position position, bool instantly = false)
        {
            _loggingHandler.LogInfo("", "TDSCamera.SetPosition");
            Cam.Position = position;

            if (instantly)
            {
                Cam.Render(true, false, 0);
            }
            _loggingHandler.LogInfo("", "TDSCamera.SetPosition", true);
        }

        public void Spectate(IEntityBase entity)
        {
            //Todo
            if (entity is IPedBase ped)
                Spectate(ped);
        }

        public void Spectate(IPedBase ped)
        {
            if (SpectatingEntity != null)
            {
                Cam.Detach();
            }

            SpectatingEntity = ped;
            Cam.AttachTo(ped, PedBone.SKEL_Head, 0, -2f, 0.3f, true);

            _modAPI.Streaming.SetFocusEntity(ped);
            _camerasHandler.FocusAtPos = null;
        }

        #endregion Public Methods
    }
}
