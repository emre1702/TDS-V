using TDS_Client.Instance.Utility;

namespace TDS_Client.Manager.Utility
{
    static class CameraManager
    {
        public static TDSCamera BetweenRoundsCam { get; set; } = new TDSCamera();
        public static TDSCamera FreeCam { get; set; } = new TDSCamera();
    }
}
