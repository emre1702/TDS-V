using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Utility;

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
                if (value)
                {
                    MapLimitManager.Start();
                    if (!_inFight)
                    {
                        Damagesys.ResetLastHP();
                        FloatingDamageInfo.UpdateAllPositions();
                    }
                }  
                else
                {
                    MapLimitManager.Stop();
                    if (_inFight)
                    {
                        FloatingDamageInfo.RemoveAll();
                    }
                }
                _inFight = value;
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