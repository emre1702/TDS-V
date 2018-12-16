using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Damage
{
    static class DeathmatchInfo
    {
        public static void HittedOpponent()
        {
            MainBrowser.PlayHitsound();
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
