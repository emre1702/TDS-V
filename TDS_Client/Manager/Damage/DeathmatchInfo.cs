using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Damage
{
    static class DeathmatchInfo
    {
        public static void HittedOpponent(Player hitted, int damage)
        {
            if (Settings.Hitsound)
                MainBrowser.PlayHitsound();

            if (Settings.FloatingDamageInfo)
                FloatingDamageInfo.Create(hitted, damage);
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
