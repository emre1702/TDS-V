using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Entities.Draw.Scaleform;

namespace TDS_Client.Handler.Draw
{
    public class MidsizedMessageHandler
    {
        private int _initTimeMs;
        private int _msgDurationMs;
        private bool _animatedOut;
        private int _msgBgColor;
        private BasicScaleform _midsizedScaleform;

        private BasicScaleform MidsizedScaleform
        {
            get
            {
                if (_midsizedScaleform == null)
                    _midsizedScaleform = new BasicScaleform(ScaleformName.MIDSIZED_MESSAGE, _modAPI);
                return _midsizedScaleform;
            }
        }

        private readonly IModAPI _modAPI;
        private readonly TimerHandler _timerHandler;

        public MidsizedMessageHandler(IModAPI modAPI, TimerHandler timerHandler)
        {
            _modAPI = modAPI;
            _timerHandler = timerHandler;
        }

        public void ShowMidsizedMessage(string title, string message, int time = 5000)
        {
            MidsizedScaleform.Call(ScaleformFunction.SHOW_MIDSIZED_MESSAGE, title, message);
            InitCommonSettings(time);
        }

        public void ShowMidsizedShardMessage(string title, string message, int bgColor, bool useDarkerShard, bool condensed, int time = 5000)
        {
            MidsizedScaleform.Call(ScaleformFunction.SHOW_SHARD_MIDSIZED_MESSAGE, title, message, bgColor, useDarkerShard, condensed);
            InitCommonSettings(time);
            _msgBgColor = bgColor;
        }

        private void InitCommonSettings(int time)
        {
            _initTimeMs = _timerHandler.ElapsedMs;
            _msgDurationMs = time;
            _animatedOut = false;
        }

        public void Render()
        {
            if (_midsizedScaleform == null)
                return;
            if (_initTimeMs == 0)
                return;

            _midsizedScaleform.RenderFullscreen();
            if (_timerHandler.ElapsedMs - _initTimeMs > _msgDurationMs)
            {
                if (!_animatedOut)
                {
                    _midsizedScaleform.Call(ScaleformFunction.SHARD_ANIM_OUT, _msgBgColor);
                    _animatedOut = true;
                    _msgDurationMs += 750;
                }
                else
                {
                    _initTimeMs = 0;
                    _midsizedScaleform.Dispose();
                    _midsizedScaleform = null;
                }
            }
        }
    }
}
