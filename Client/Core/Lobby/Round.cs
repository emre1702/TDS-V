using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Draw;
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
                        FightInfo.Reset();
                        FloatingDamageInfo.UpdateAllPositions();
                        FiringMode.Start();
                        Browser.Angular.Main.ToggleRoundStats(true);
                    }
                }  
                else
                {
                    MapLimitManager.Stop();
                    if (_inFight)
                    {
                        FloatingDamageInfo.RemoveAll();
                        FiringMode.Stop();
                        Browser.Angular.Main.ToggleRoundStats(false);
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
