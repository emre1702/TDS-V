using System;
using TDS_Client.Default;
using TDS_Client.Instance.Draw.Scaleform;

namespace TDS_Client.Manager.Draw
{
    class MidsizedMessage
    {
        private static int initTimeMs;
        private static int msgDurationMs;
        private static bool animatedOut;
        private static int msgBgColor;
        private static BasicScaleform fmidsizedScaleform;

        private static BasicScaleform midsizedScaleform
        {
            get
            {
                if (fmidsizedScaleform == null)
                    fmidsizedScaleform = new BasicScaleform(DScaleformName.MIDSIZED_MESSAGE);
                return fmidsizedScaleform;
            }
        }

        public static void ShowMidsizedMessage(string title, string message, int time = 5000)
        {
            midsizedScaleform.Call(DScaleformFunction.SHOW_MIDSIZED_MESSAGE, title, message);
            InitCommonSettings(time);
        }

        public static void ShowMidsizedShardMessage(string title, string message, int bgColor, bool useDarkerShard, bool condensed, int time = 5000)
        {
            midsizedScaleform.Call(DScaleformFunction.SHOW_SHARD_MIDSIZED_MESSAGE, title, message, bgColor, useDarkerShard, condensed);
            InitCommonSettings(time);
            msgBgColor = bgColor;
        }

        private static void InitCommonSettings(int time)
        {
            initTimeMs = Environment.TickCount;
            msgDurationMs = time;
            animatedOut = false;
        }

        public static void Render()
        {
            if (fmidsizedScaleform == null)
                return;
            if (initTimeMs == 0)
                return;

            fmidsizedScaleform.RenderFullscreen();
            if (Environment.TickCount - initTimeMs > msgDurationMs)
            {
                if (!animatedOut)
                {
                    fmidsizedScaleform.Call(DScaleformFunction.SHARD_ANIM_OUT, msgBgColor);
                    animatedOut = true;
                    msgDurationMs += 750;
                }
                else
                {
                    initTimeMs = 0;
                    fmidsizedScaleform.Destroy();
                    fmidsizedScaleform = null;
                }
            }
        }
    }
}
