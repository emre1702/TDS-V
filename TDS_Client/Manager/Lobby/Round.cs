using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Instance.Draw.Dx;

namespace TDS_Client.Manager.Lobby
{
    static class Round
    {
        private static bool inFight;

        //public static bool IsSpectator;
        //private static string currentMap;

        public static bool InFight
        {
            get => inFight;
            set {
                inFight = value;
                if (!value)
                    MapLimitManager.Stop();
            }
        }

        public static void Reset(bool removemapinfo)
        {
            //StopSpectate();
            //StopCountdown();
            if (removemapinfo)
                MapInfo.RemoveMapInfo();
        }
    }
}
