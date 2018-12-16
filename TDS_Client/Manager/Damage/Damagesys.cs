using RAGE;
using RAGE.Elements;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;

namespace TDS_Client.Manager.Damage
{
    static class Damagesys
    {
        private static int lastTotalHP = 0;

        // Body parts: https://pastebin.com/AGQWgCct

        public static void ShowBloodscreenIfNecessary()
        {
            if (!Round.InFight)
                return;
            if (!Settings.ShowBloodscreen)
                return;
            int currentTotalHP = Player.LocalPlayer.GetHealth() + Player.LocalPlayer.GetArmour();
            if (currentTotalHP == lastTotalHP)
                return;

            if (currentTotalHP < lastTotalHP)
                MainBrowser.ShowBloodscreen();
            lastTotalHP = currentTotalHP;
        }

        public static void OnWeaponShot(Vector3 targetPos, Player target, CancelEventArgs cancel)
        {
            if (target == null)
                return;
            cancel.Cancel = true;
            CallRemote(DToServerEvent.HitOtherPlayer, target, false);
        }

        public static void Reset()
        {
            lastTotalHP = 0;
        }
       
    }
}
