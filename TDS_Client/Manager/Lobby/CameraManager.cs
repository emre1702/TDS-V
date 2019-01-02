using RAGE;
using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Manager.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    static class CameraManager
    {
        private static Camera cam;
        private static TDSTimer timer;

        public static void SetToMapCenter(Vector3 mapcenter)
        {
            Cam.SetCamCoord(cam.Id, mapcenter.X, mapcenter.Y, mapcenter.Z + 110);
            Cam.PointCamAtCoord(cam.Id, mapcenter.X, mapcenter.Y, mapcenter.Z);
            Cam.SetCamActive(cam.Id, true);
            Cam.RenderScriptCams(true, true, 3000, true, true, 0);
        }

        public static void SetGoTowardsPlayer(int? time = null)
        {
            //timer = null;
            Cam.RenderScriptCams(false, true, time ?? (int)(Settings.CountdownTime * 0.9), true, true, 0);
        }

        public static void SetTimerTowardsPlayer(uint execafterms)
        {
            timer = new TDSTimer(() => SetGoTowardsPlayer(), execafterms, 1);
        }

        public static void StopCountdown()
        {
            Cam.RenderScriptCams(false, false, 0, true, true, 0);
        }

        public static void Stop()
        {
            timer?.Kill();
            timer = null;
        }
    }
}
