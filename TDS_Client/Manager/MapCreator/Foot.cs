using TDS_Client.Instance.Utility;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Foot
    {
        public static void Start()
        {
            var cam = CameraManager.FreeCam;
            var player = RAGE.Elements.Player.LocalPlayer;

            player.Position = cam.Position;
            player.SetHeading(cam.Rotation.Z);
            player.FreezePosition(false);
            player.SetVisible(true, false);
            player.SetCollision(true, true);

            TDSCamera.RenderBack();
        }

        public static void Stop()
        {
            var player = RAGE.Elements.Player.LocalPlayer;
            player.FreezePosition(true);
            player.SetVisible(false, false);
            player.SetCollision(false, false);
        }
    }
}
