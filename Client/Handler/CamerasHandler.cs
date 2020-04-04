using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Instance.Utility;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler
{
    public class CamerasHandler
    {
        public TDSCamera BetweenRoundsCam { get; set; }
        public TDSCamera FreeCam { get; set; }
        public TDSCamera SpectateCam { get; set; }

        public TDSCamera ActiveCamera { get; set; }

        private readonly IModAPI _modAPI;

        public CamerasHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;

            modAPI.Cam.RenderScriptCams(false, false, 0, true, false, 0);
            modAPI.Cam.DestroyAllCams();

            BetweenRoundsCam = new TDSCamera(modAPI);
            SpectateCam = new TDSCamera(modAPI);
        }

        public Position3D GetCurrentCamPos()
        {
            return ActiveCamera?.Position ?? _modAPI.Cam.GetGameplayCamCoord();
        }

        public Position3D GetCurrentCamRot()
        {
            return ActiveCamera?.Rotation ?? _modAPI.Cam.GetGameplayCamRot();
        }
    }
}
