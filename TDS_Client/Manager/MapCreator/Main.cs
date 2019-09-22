using TDS_Client.Enum;
using TDS_Client.Manager.Draw;

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
            InstructionalButtonManager.Reset();
            Binds.RemoveGeneral();
            Freecam.Stop();
            Foot.Start();
            Browser.Angular.Main.ToggleMapCreator(false);
            Browser.Angular.Main.ToggleFreeroam(false);
            ObjectsManager.Stop();
            MarkerManager.Stop();
            ObjectPreview.Stop();
            Browser.Angular.MapCreatorObjectChoice.Stop();
        }

        public static void ToggleFreecam(EKey _ = EKey.A)
        {
            if (Browser.Angular.Shared.InInput)
                return;

            InstructionalButtonManager.Reset();
            Binds.SetGeneral();
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
