using RAGE;
using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Manager.Utility;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    static class CameraManager
    {
        private static int cam = Cam.CreateCam("DEFAULT_SCRIPTED_CAMERA", false);
        private static TDSTimer timer;

        public static void SetToMapCenter(Vector3 mapcenter)
        {
            Cam.SetCamCoord(cam, mapcenter.X, mapcenter.Y, mapcenter.Z + 110);
            Cam.PointCamAtCoord(cam, mapcenter.X, mapcenter.Y, mapcenter.Z);
            Cam.SetCamActive(cam, true);
            Cam.RenderScriptCams(true, true, Settings.MapChooseTime, true, true, 0);
        }

        public static void SetGoTowardsPlayer(uint? time = null)
        {
            //timer = null;
            Cam.RenderScriptCams(false, true, (int)(time ?? (Settings.CountdownTime * 0.9)), true, true, 0);
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
            if (timer == null)
                return;
            timer.Kill();
            timer = null;
        }
    }
}
