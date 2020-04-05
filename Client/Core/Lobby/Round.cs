using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Lobby
{
    internal static class Round
    {
        

        public static bool IsSpectator { get; set; }

        public static void StopFight()
        {
            InFight = false;
            IsSpectator = false;
            FightInfo.Reset();
            Countdown.Stop();
        }

        public static void Reset(bool removemapinfo)
        {
            StopFight();
            Spectate.Stop();
            if (removemapinfo)
                MapDataManager.RemoveMapInfo();
        }
    }
}
