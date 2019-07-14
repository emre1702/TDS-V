using TDS_Client.Manager.Damage;

namespace TDS_Client.Manager.Lobby
{
    internal static class Round
    {
        private static bool _inFight;

        public static bool InFight
        {
            get => _inFight;
            set
            {
                _inFight = value;
                if (value)
                    MapLimitManager.Start();
                else
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