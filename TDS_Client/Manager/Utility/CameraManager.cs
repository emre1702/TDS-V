using RAGE.Game;
using TDS_Client.Instance.Utility;

namespace TDS_Client.Manager.Utility
{
    static class CameraManager
    {
        public static TDSCamera BetweenRoundsCam { get; set; }
        public static TDSCamera FreeCam { get; set; }
        public static TDSCamera SpectateCam { get; set; }

        public static void Init()
        {
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            Cam.DestroyAllCams(false);

            BetweenRoundsCam = new TDSCamera();
            FreeCam = new TDSCamera();
            SpectateCam = new TDSCamera();
        }
    }
}
