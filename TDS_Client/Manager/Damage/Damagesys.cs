using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System.Linq;
using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using static RAGE.Events;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    static class Damagesys
    {
        private static int lastTotalHP = 0;

        public static int CurrentWeaponDamage;

        // Body parts: https://pastebin.com/AGQWgCct

        public static void ShowBloodscreenIfNecessary()
        {
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

            Player hitted = GetHittedPlayer(targetPos);

            if (hitted == null)
            {
                // debug //
                if (target != null)
                    Chat.Output("Target is there, hitted is null!");
                //////////
                return;
            }
            EventsSender.Send(DToServerEvent.HitOtherPlayer, hitted.Name, false, CurrentWeaponDamage);

            FloatingDamageInfo.Create(hitted, CurrentWeaponDamage);

            // debug //
            if (target == hitted)
                Chat.Output("Target and hitted are the same!");
            else
                Chat.Output("Target and hitted are different!");
            
            int damagedBone = 0;
            hitted.GetLastDamageBone(ref damagedBone);
            Chat.Output("Hitted Bone: " + damagedBone);
            //////////
        }

        private static Player GetHittedPlayer(Vector3 targetPos)
        {
            Vector3 startpos = Player.LocalPlayer.GetBoneCoords(6286, 0, 0, 0);
            Vector3 endpos = Vector3.Lerp(startpos, targetPos, 1.05f);
            int rayHandle = Shapetest.StartShapeTestRay(startpos.X, startpos.Y, startpos.Z, endpos.X, endpos.Y, endpos.Z, 8, Player.LocalPlayer.Handle, 0);
            int hit = 0;
            int hitEntityHandle = 0;
            Shapetest.GetShapeTestResult(rayHandle, ref hit, endpos, startpos, ref hitEntityHandle);
            if (hit != 0)
                return Entities.Players.All.FirstOrDefault(p => p.Handle == hitEntityHandle);
            return null;
        }

        public static void ResetLastHP()
        {
            lastTotalHP = Player.LocalPlayer.GetHealth() + Player.LocalPlayer.GetArmour();
        }
       
    }
}
