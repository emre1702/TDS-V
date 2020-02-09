using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto.Map;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.Lobby
{
    internal static class LobbyCam
    {
        private static TDSTimer _timer;

        public static void SetToMapCenter(Position3DDto mapcenter)
        {
            var cam = CameraManager.BetweenRoundsCam;
            cam.PointCamAtCoord(mapcenter.X, mapcenter.Y, mapcenter.Z);
            cam.Activate();
            cam.RenderToPosition(mapcenter.X, mapcenter.Y, mapcenter.Z + 110);
        }

        public static void SetGoTowardsPlayer(int? time = null)
        {
            TDSCamera.RenderBack(true, (int)(time ?? (Settings.CountdownTime * 0.9)));
        }

        public static void SetTimerTowardsPlayer(uint execafterms)
        {
            _timer = new TDSTimer(() => SetGoTowardsPlayer(), execafterms, 1);
        }

        public static void StopCountdown()
        {
            TDSCamera.RenderBack(false, 0);
        }

        public static void Stop()
        {
            _timer?.Kill();
            _timer = null;
        }
    }
}
