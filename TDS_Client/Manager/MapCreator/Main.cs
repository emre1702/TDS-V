using TDS_Client.Enum;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.MapCreator
{
    class Main
    {
        public static void Start()
        {
            Browser.Angular.Main.ToggleMapCreator(true);
            Browser.Angular.Main.ToggleFreeroam(true);
            Binds.SetGeneral();
            ToggleFreecam();
            ObjectsManager.Start();
            MarkerManager.Start();

            RAGE.Elements.Player.LocalPlayer.SetInvincible(true);
        }

        public static void Stop()
        {
            Binds.RemoveGeneral();
            Freecam.Stop();
            Foot.Start();
            Browser.Angular.Main.ToggleMapCreator(false);
            Browser.Angular.Main.ToggleFreeroam(false);
            ObjectsManager.Stop();
            MarkerManager.Stop();
        }

        public static void ToggleFreecam(EKey _ = EKey.A)
        {
            if (Freecam.IsActive)
            {
                Freecam.Stop();
                Foot.Start();
            }
            else
            {
                Foot.Stop();
                Freecam.Start();
            }
        }
    }
}
