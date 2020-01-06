using RAGE.Elements;
using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Damage
{
    internal static class DeathmatchInfo
    {
        public static void HittedOpponent(Player hitted, int damage)
        {
            if (Settings.PlayerSettings.Hitsound)
                MainBrowser.PlayHitsound();

            if (Settings.PlayerSettings.FloatingDamageInfo && hitted != null)
                new FloatingDamageInfo(hitted, damage);
        }

        public static void WeaponShot()
        {
            int weapon = 0;
            int ammo = 0;
            Player.LocalPlayer.GetCurrentWeapon(ref weapon, true);
            Player.LocalPlayer.GetAmmoInClip((uint)weapon, ref ammo);
            int mag = Player.LocalPlayer.GetAmmoInWeapon((uint)weapon);

            Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Ammo, ammo); 
            Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Mag, mag);
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