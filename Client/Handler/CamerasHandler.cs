using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler
{
    public class CamerasHandler : ServiceBase
    {
        public TDSCamera BetweenRoundsCam { get; set; }
        public TDSCamera FreeCam { get; set; }
        public TDSCamera SpectateCam { get; set; }

        public TDSCamera ActiveCamera { get; set; }

        public Position3D FocusAtPos { get; set; }

        public SpectatingHandler Spectating { get; }

        public CamerasHandler(IModAPI modAPI, LoggingHandler loggingHandler, UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler, 
            DeathHandler deathHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            Spectating = new SpectatingHandler(modAPI, remoteEventsSender, bindsHandler, this, deathHandler, eventsHandler, utilsHandler);

            modAPI.Cam.Render(false, false, 0);
            modAPI.Cam.DestroyAllCams();

            BetweenRoundsCam = new TDSCamera(modAPI, loggingHandler, this, utilsHandler);
            SpectateCam = new TDSCamera(modAPI, loggingHandler, this, utilsHandler);
        }

        public Position3D GetCurrentCamPos()
        {
            return ActiveCamera?.Position ?? ModAPI.Cam.GetGameplayCamCoord();
        }

        public Position3D GetCurrentCamRot()
        {
            return ActiveCamera?.Rotation ?? ModAPI.Cam.GetGameplayCamRot();
        }

        public void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = Spectating.SpectatingEntity;
            ActiveCamera?.Deactivate();
            if (spectatingEntity != null)
            {
                ModAPI.Streaming.SetFocusEntity(spectatingEntity);
                SpectateCam.Activate();
                ModAPI.Cam.Render(true, ease, easeTime);
            }
            else
            {
                RemoveFocusArea();
                ModAPI.Cam.Render(false, ease, easeTime);
                ActiveCamera = null;
            }
        }

        public void RemoveFocusArea()
        {
            ModAPI.Streaming.SetFocusEntity(ModAPI.LocalPlayer);
            FocusAtPos = null;
        }

        public void SetFocusArea(Position3D pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                ModAPI.Streaming.SetFocusArea(pos, 0, 0, 0);
                FocusAtPos = pos;
            }
        }
    }
}
