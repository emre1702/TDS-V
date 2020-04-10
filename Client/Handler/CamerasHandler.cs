using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler
{
    public class CamerasHandler
    {
        public TDSCamera BetweenRoundsCam { get; set; }
        public TDSCamera FreeCam { get; set; }
        public TDSCamera SpectateCam { get; set; }

        public TDSCamera ActiveCamera { get; set; }

        public Position3D FocusAtPos { get; set; }

        private readonly IModAPI _modAPI;
        private readonly SpectatingHandler _spectatingHandler;

        public CamerasHandler(IModAPI modAPI, UtilsHandler utilsHandler, SpectatingHandler spectatingHandler)
        {
            _modAPI = modAPI;
            _spectatingHandler = spectatingHandler;

            modAPI.Cam.RenderScriptCams(false, false, 0, true, false, 0);
            modAPI.Cam.DestroyAllCams();

            BetweenRoundsCam = new TDSCamera(modAPI, this, utilsHandler, spectatingHandler);
            SpectateCam = new TDSCamera(modAPI, this, utilsHandler, spectatingHandler);
        }

        public Position3D GetCurrentCamPos()
        {
            return ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
        }

        public Position3D GetCurrentCamRot()
        {
            return ActiveCamera?.Rotation ?? _modAPI.Cam.GetGameplayCamRot();
        }

        public void RenderBack(bool ease = false, int easeTime = 0)
        {
            var spectatingEntity = _spectatingHandler.SpectatingEntity;
            ActiveCamera?.Deactivate();
            if (spectatingEntity != null)
            {
                _modAPI.Streaming.SetFocusEntity(spectatingEntity);
                SpectateCam.Activate();
                _modAPI.Cam.Render(true, ease, easeTime);
            }
            else
            {
                RemoveFocusArea();
                _modAPI.Cam.Render(false, ease, easeTime);
                ActiveCamera = null;
            }
        }

        public void RemoveFocusArea()
        {
            _modAPI.Streaming.SetFocusEntity(_modAPI.LocalPlayer);
            FocusAtPos = null;
        }

        public void SetFocusArea(Position3D pos)
        {
            if (FocusAtPos is null || FocusAtPos.DistanceTo(pos) >= 50)
            {
                _modAPI.Streaming.SetFocusArea(pos, 0, 0, 0);
                FocusAtPos = pos;
            }
        }
    }
}
