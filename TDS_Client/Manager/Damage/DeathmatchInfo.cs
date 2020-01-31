using RAGE.Elements;
using System;
using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Damage
{
    internal static class DeathmatchInfo
    {
        private static uint _currentWeapon;
        private static int _lastAmmo = 0;
        private static int _lastMag = 0;

        public static void Init()
        {
            CustomEventManager.OnWeaponChange += CustomEventManager_OnWeaponChange;
        }

        public static void HittedOpponent(Player hitted, int damage)
        {
            if (Settings.PlayerSettings.Hitsound)
                MainBrowser.PlayHitsound();

            if (Settings.PlayerSettings.FloatingDamageInfo && hitted != null)
                new FloatingDamageInfo(hitted, damage);
        }

        public static void OnTick()
        {
            uint weapon = Player.LocalPlayer.GetSelectedWeapon();
            int ammo = 0;
            Player.LocalPlayer.GetAmmoInClip(weapon, ref ammo);
            int mag = Player.LocalPlayer.GetAmmoInWeapon(weapon);

            mag -= ammo;

            if (_lastAmmo == ammo && _lastMag == mag)
                return;

            if (_lastAmmo != ammo)
            {
                Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Ammo, ammo);
                _lastAmmo = ammo;
            }

            if (_lastMag != mag)
            {
                Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Mag, mag);
                _lastMag = mag;
            }
        }

        private static void CustomEventManager_OnWeaponChange(uint _, uint newWeaponHash)
        {
            if (_currentWeapon == newWeaponHash)
                return;

            _currentWeapon = newWeaponHash;
            _lastAmmo = 0;
            _lastMag = 0;
        }

        /*
         * let deathmatchinfodata = {
    damage: 0,
    kills: 0,
    assists: 0
}
         * function restartDeathmatchInfo() {
    deathmatchinfodata.damage = 0;
    deathmatchinfodata.kills = 0;
    deathmatchinfodata.assists = 0;
}*/
    }
}
