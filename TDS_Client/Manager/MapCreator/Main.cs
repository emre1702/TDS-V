using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Common.Instance.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Main
    {
        public static void Start()
        {
            Angular.ToggleMapCreator(true);
            Angular.ToggleFreeroam(true);
            Binds.SetGeneral();
            ToggleFreecam();

            // TEST //
            new TDSTimer(() =>
            {
                var obj = new RAGE.Elements.MapObject(RAGE.Game.Misc.GetHashKey("prop_mp_placement_med"),
                   new RAGE.Vector3(0, 0, 80), new RAGE.Vector3(), dimension: RAGE.Elements.Player.LocalPlayer.Dimension);
                ObjectsManager.Add(obj);
            }, 5000);
            /////////
        }

        public static void Stop()
        {
            Binds.RemoveGeneral();
            Freecam.Stop();
            Foot.Start();
            Angular.ToggleMapCreator(false);
            Angular.ToggleFreeroam(false);
            Blips.Clear();
            ObjectsManager.Clear();
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
