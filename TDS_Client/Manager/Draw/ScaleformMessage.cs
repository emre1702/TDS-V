using TDS_Client.Default;
using TDS_Client.Instance.Draw.Scaleform;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Draw.Scaleform
{
    internal static class ScaleformMessage
    {
        private static ulong initTimeMs;
        private static ulong msgDurationMs;
        private static bool animatedOut;
        private static BasicScaleform fscaleform;

        private static BasicScaleform scaleform
        {
            get
            {
                if (fscaleform == null)
                    fscaleform = new BasicScaleform(DScaleformName.MP_BIG_MESSAGE_FREEMODE);
                return fscaleform;
            }
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
            //todo Test this, showed "undefined"?
            scaleform.Call(DScaleformFunction.SHOW_WASTED_MP_MESSAGE, "wasted");
            InitCommonSettings(time);
        }

        private static void InitCommonSettings(ulong time)
        {
            initTimeMs = TimerManager.ElapsedTicks;
            msgDurationMs = time;
            animatedOut = false;
        }

        public static void Render()
        {
            if (fscaleform == null)
                return;
            if (initTimeMs == 0)
                return;

            fscaleform.RenderFullscreen();
            if (TimerManager.ElapsedTicks - initTimeMs > msgDurationMs)
            {
                if (!animatedOut)
                {
                    fscaleform.Call(DScaleformFunction.TRANSITION_OUT);
                    animatedOut = true;
                    msgDurationMs += 750;
                }
                else
                {
                    initTimeMs = 0;
                    fscaleform.Destroy();
                    fscaleform = null;
                }
            }
        }
    }
}