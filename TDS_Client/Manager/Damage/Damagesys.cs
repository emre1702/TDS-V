using RAGE;
using RAGE.Elements;
using System.Linq;
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
            cancel.Cancel = true;

            Vector3 startpos = Player.LocalPlayer.GetBoneCoords(6286, 0, 0, 0);
            Vector3 endpos = Vector3.Lerp(startpos, targetPos, 1.05f);
            int rayHandle = RAGE.Game.Shapetest.StartShapeTestRay(startpos.X, startpos.Y, startpos.Z, endpos.X, endpos.Y, endpos.Z, 8, Player.LocalPlayer.Handle, 0);
            int hit = 0;
            int hitEntityHandle = 0;
            RAGE.Game.Shapetest.GetShapeTestResult(rayHandle, ref hit, endpos, startpos, ref hitEntityHandle);
            if (hit != 0)
            {
                Player hitted = Entities.Players.All.FirstOrDefault(p => p.Handle == hitEntityHandle);
                if (hitted != null)
                    CallRemote(DToServerEvent.HitOtherPlayer, hitted.Name, false);
            }
        }

        public static void Reset()
        {
            lastTotalHP = 0;
        }
       
    }
}
