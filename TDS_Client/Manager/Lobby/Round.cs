using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Damage;

namespace TDS_Client.Manager.Lobby
{
    static class Round
    {
        private static bool inFight;

        public static bool InFight
        {
            get => inFight;
            set {
                inFight = value;
                if (!value)
                    MapLimitManager.Stop();
            }
        }
        public static bool IsSpectator { get; set; }

        public static void StopFight()
        {
            InFight = false;
            IsSpectator = false;
            Damagesys.ResetLastHP();
            Countdown.Stop();
        }

        public static void Reset(bool removemapinfo)
        {
            StopFight();
            Spectate.Stop();
            if (removemapinfo)
                MapInfo.RemoveMapInfo();
        }
    }
}
