using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Entities.Draw.Scaleform;

namespace TDS_Client.Handler.Draw
{
    public class ScaleformMessageHandler
    {
        private int _initTimeMs;
        private int _msgDurationMs;
        private bool _animatedOut;
        private BasicScaleform _scaleform;

        private BasicScaleform Scaleform
        {
            get
            {
                if (_scaleform == null)
                    _scaleform = new BasicScaleform(ScaleformName.MP_BIG_MESSAGE_FREEMODE, _modAPI);
                return _scaleform;
            }
        }

        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;

        public ScaleformMessageHandler(IModAPI modAPI, SettingsHandler settingsHandler, TimerHandler timerHandler)
        {
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _timerHandler = timerHandler;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(Render));
        }

        public void ShowWeaponPurchasedMessage(string title, string weaponName, int weaponHash, int time = 5000)
        {
            Scaleform.Call(ScaleformFunction.SHOW_WEAPON_PURCHASED, title, weaponName, weaponHash);
            InitCommonSettings(time);
        }

        public void ShowPlaneMessage(string title, string planeName, string planeHash, int time = 5000)
        {
            Scaleform.Call(ScaleformFunction.SHOW_PLANE_MESSAGE, title, planeName, planeHash);
            InitCommonSettings(time);
        }

        public void ShowShardMessage(string title, string message, string titleColor, int bgColor, int time = 5000)
        {
            Scaleform.Call(ScaleformFunction.SHOW_SHARD_CENTERED_MP_MESSAGE, title, message, titleColor, bgColor);
            InitCommonSettings(time);
        }

        public void ShowWastedMessage(int time = 5000)
        {
            Scaleform.Call(ScaleformFunction.SHOW_SHARD_WASTED_MP_MESSAGE, "~r~Wasted", _settingsHandler.Language.YOU_DIED, 5, true, true);
            InitCommonSettings(time);
        }

        private void InitCommonSettings(int time)
        {
            _initTimeMs = _timerHandler.ElapsedMs;
            _msgDurationMs = time;
            _animatedOut = false;
        }

        public void Render(int currentMs)
        {
            if (_scaleform == null)
                return;
            if (_initTimeMs == 0)
                return;

            _scaleform.RenderFullscreen();
            if (currentMs - _initTimeMs > _msgDurationMs)
            {
                if (!_animatedOut)
                {
                    _scaleform.Call(ScaleformFunction.TRANSITION_OUT);
                    _animatedOut = true;
                    _msgDurationMs += 750;
                }
                else
                {
                    _initTimeMs = 0;
                    _scaleform.Dispose();
                    _scaleform = null;
                }
            }
        }
    }
}
