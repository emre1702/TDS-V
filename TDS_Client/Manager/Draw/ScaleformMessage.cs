using TDS_Client.Default;
using TDS_Client.Instance.Draw.Scaleform;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Draw.Scaleform
{
    internal static class ScaleformMessage
    {
        private static ulong _initTimeMs;
        private static ulong _msgDurationMs;
        private static bool _animatedOut;
        private static BasicScaleform _fscaleform;

        private static BasicScaleform scaleform
        {
            get
            {
                if (_fscaleform == null)
                    _fscaleform = new BasicScaleform(DScaleformName.MP_BIG_MESSAGE_FREEMODE);
                return _fscaleform;
            }
        }

        static ScaleformMessage()
        {
            TickManager.Add(Render);
        }

        public static void ShowWeaponPurchasedMessage(string title, string weaponName, int weaponHash, ulong time = 5000)
        {
            scaleform.Call(DScaleformFunction.SHOW_WEAPON_PURCHASED, title, weaponName, weaponHash);
            InitCommonSettings(time);
        }

        public static void ShowPlaneMessage(string title, string planeName, string planeHash, ulong time = 5000)
        {
            scaleform.Call(DScaleformFunction.SHOW_PLANE_MESSAGE, title, planeName, planeHash);
            InitCommonSettings(time);
        }

        public static void ShowShardMessage(string title, string message, string titleColor, int bgColor, ulong time = 5000)
        {
            scaleform.Call(DScaleformFunction.SHOW_SHARD_CENTERED_MP_MESSAGE, title, message, titleColor, bgColor);
            InitCommonSettings(time);
        }

        public static void ShowWastedMessage(ulong time = 5000)
        {
            scaleform.Call(DScaleformFunction.SHOW_SHARD_WASTED_MP_MESSAGE, "~r~Wasted", "You died.", 5, true, true);
            InitCommonSettings(time);
        }

        private static void InitCommonSettings(ulong time)
        {
            _initTimeMs = TimerManager.ElapsedTicks;
            _msgDurationMs = time;
            _animatedOut = false;
        }

        public static void Render()
        {
            if (_fscaleform == null)
                return;
            if (_initTimeMs == 0)
                return;

            _fscaleform.RenderFullscreen();
            if (TimerManager.ElapsedTicks - _initTimeMs > _msgDurationMs)
            {
                if (!_animatedOut)
                {
                    _fscaleform.Call(DScaleformFunction.TRANSITION_OUT);
                    _animatedOut = true;
                    _msgDurationMs += 750;
                }
                else
                {
                    _initTimeMs = 0;
                    _fscaleform.Dispose();
                    _fscaleform = null;
                }
            }
        }
    }
}