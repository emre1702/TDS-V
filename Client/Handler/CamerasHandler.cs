using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler
{
    public class CamerasHandler : ServiceBase
    {
        #region Public Constructors

        public CamerasHandler(IModAPI modAPI, LoggingHandler loggingHandler, UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler,
            DeathHandler deathHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            Spectating = new SpectatingHandler(modAPI, loggingHandler, remoteEventsSender, bindsHandler, this, deathHandler, eventsHandler, utilsHandler);

            modAPI.Cam.Render(false, false, 0);
            modAPI.Cam.DestroyAllCams();

            BetweenRoundsCam = new TDSCamera(nameof(BetweenRoundsCam), modAPI, loggingHandler, this, utilsHandler);
            SpectateCam = new TDSCamera(nameof(SpectateCam), modAPI, loggingHandler, this, utilsHandler);
        }

        #endregion Public Constructors

        #region Public Properties

        public TDSCamera ActiveCamera { get; set; }
        public TDSCamera BetweenRoundsCam { get; set; }
        public Position FocusAtPos { get; set; }
        public TDSCamera FreeCam { get; set; }
        public TDSCamera SpectateCam { get; set; }
        public SpectatingHandler Spectating { get; }

        #endregion Public Properties

        #region Public Methods

        public Position GetCurrentCamPos()
        {
            return ActiveCamera?.Position ?? ModAPI.Cam.GetGameplayCamCoord();
        }

        public Position GetCurrentCamRot()
        {
            return ActiveCamera?.Rotation ?? ModAPI.Cam.GetGameplayCamRot();
        }

        public void RemoveFocusArea()
        {
            ModAPI.Streaming.ClearFocus();
            FocusAtPos = null;
        }

        public void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = Spectating.SpectatingEntity;
            if (spectatingEntity != null)
            {
                ModAPI.Streaming.SetFocusEntity(spectatingEntity);
                SpectateCam.Spectate(spectatingEntity);
                SpectateCam.Activate();
                SpectateCam.Render(ease, easeTime);
            }
            else
            {
                ActiveCamera?.Deactivate();
                RemoveFocusArea();
                ModAPI.Cam.Render(false, ease, easeTime);
                ActiveCamera = null;
            }
        }

        public void SetFocusArea(Position pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                ModAPI.Streaming.SetFocusArea(pos, 0, 0, 0);
                FocusAtPos = pos;
            }
        }

        #endregion Public Methods
    }
}
