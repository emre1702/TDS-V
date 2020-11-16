using RAGE;
using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Entities;
using TDS.Client.Handler.Events;

namespace TDS.Client.Handler
{
    public class CamerasHandler : ServiceBase
    {
        public CamerasHandler(LoggingHandler loggingHandler, UtilsHandler utilsHandler, RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler,
            DeathHandler deathHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            Spectating = new SpectatingHandler(loggingHandler, remoteEventsSender, bindsHandler, this, deathHandler, eventsHandler, utilsHandler);

            RAGE.Game.Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Game.Cam.DestroyAllCams(false);

            BetweenRoundsCam = new TDSCamera(nameof(BetweenRoundsCam), loggingHandler, this, utilsHandler);
            SpectateCam = new TDSCamera(nameof(SpectateCam), loggingHandler, this, utilsHandler);
        }

        public TDSCamera ActiveCamera { get; set; }
        public TDSCamera BetweenRoundsCam { get; set; }
        public Vector3 FocusAtPos { get; set; }
        public TDSCamera FreeCam { get; set; }
        public TDSCamera SpectateCam { get; set; }
        public SpectatingHandler Spectating { get; }

        public Vector3 GetCurrentCamPos()
        {
            return ActiveCamera?.Position ?? RAGE.Game.Cam.GetGameplayCamCoord();
        }

        public Vector3 GetCurrentCamRot()
        {
            return ActiveCamera?.Rotation ?? RAGE.Game.Cam.GetGameplayCamRot(2);
        }

        public void RemoveFocusArea()
        {
            RAGE.Game.Streaming.ClearFocus();
            RAGE.Game.Streaming.ClearHdArea();
            FocusAtPos = null;
        }

        public void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = Spectating.SpectatingEntity;
            if (spectatingEntity != null)
            {
                RAGE.Game.Streaming.SetFocusEntity(spectatingEntity.Handle);
                SpectateCam.Spectate(spectatingEntity);
                SpectateCam.Activate();
                SpectateCam.Render(ease, easeTime);
            }
            else
            {
                ActiveCamera?.Deactivate();
                RemoveFocusArea();
                RAGE.Game.Cam.RenderScriptCams(false, ease, easeTime, true, false, 0);
                ActiveCamera = null;
            }
        }

        public void SetFocusArea(Vector3 pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                RAGE.Game.Streaming.SetFocusArea(pos.X, pos.Y, pos.Z, 0, 0, 0);
                RAGE.Game.Streaming.SetHdArea(pos.X, pos.Y, pos.Z, 30f);
                FocusAtPos = pos;
            }
        }
    }
}
