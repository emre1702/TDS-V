using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.Lobby
{
    public class CountdownHandler
    {
        private DxText _text;
        private TDSTimer _countdownTimer;
        private int _currentCountdownTime;

        private readonly string[] _countdownSounds = new string[] { "go", "1", "2", "3" };

        private readonly SettingsHandler _settingsHandler;
        private readonly DxHandler _dxHandler;
        private readonly IModAPI _modAPI;
        private readonly TimerHandler _timerHandler;
        private readonly BrowserHandler _browserHandler;

        public CountdownHandler(SettingsHandler settingsHandler, DxHandler dxHandler, IModAPI modAPI, TimerHandler timerHandler, BrowserHandler browserHandler, EventsHandler eventsHandler)
        {
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _modAPI = modAPI;
            _timerHandler = timerHandler;
            _browserHandler = browserHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        public void Start()
        {
            if (_settingsHandler.CountdownTime == 0)
            {
                return;
            }
            _countdownTimer?.Kill();
            _currentCountdownTime = _settingsHandler.CountdownTime;
            _countdownTimer = new TDSTimer(Refresh, 1000, (uint)_currentCountdownTime + 1);
            _text = new DxText(_dxHandler, _modAPI, _timerHandler, _currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Center);
            Refresh();
        }

        public void StartAfterwards(int timeremainingms)
        {
            _currentCountdownTime = (int)Math.Ceiling((double)timeremainingms / 1000);
            _countdownTimer?.Kill();
            _countdownTimer = new TDSTimer(() =>
            {
                if (_currentCountdownTime > 1)
                    _countdownTimer = new TDSTimer(Refresh, 1000, (uint)(_currentCountdownTime));
                Refresh();
            }, (uint)(_currentCountdownTime - timeremainingms), 1);
            _text = new DxText(_dxHandler, _modAPI, _timerHandler, _currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Center);
            Refresh();
        }

        public void End(bool showGo = true)
        {
            _currentCountdownTime = 0;
            if (showGo)
            {
                _countdownTimer?.Kill();
                if (_text == null)
                    _text = new DxText(_dxHandler, _modAPI, _timerHandler, "GO", 0.5f, 0.2f, 2f, Color.White, alignmentX: AlignmentX.Center, alignmentY: AlignmentY.Center);
                else
                {
                    _text.Text = "GO";
                    _text.SetScale(2f);
                }
                PlaySound();
                _countdownTimer = new TDSTimer(Stop, 2000, 1);
            }
            else
                Stop();
        }

        public void Stop()
        {
            _text?.Remove();
            _text = null;
            _countdownTimer?.Kill();
            _countdownTimer = null;
        }

        private void Refresh()
        {
            if (--_currentCountdownTime <= 0)
            {
                return;
            }
            if (_text == null)
                return;
            _text.Text = _currentCountdownTime.ToString();
            if (_currentCountdownTime <= 5)
            {
                _text.SetScale(2f);
                _text.BlendScale(6f, 1000);
                _text.SetRelativeY(0.2f);
                PlaySound();
            }
            else
            {
                _text.SetScale(1f);
            }
        }

        private void PlaySound()
        {
            if (_countdownSounds.Length > _currentCountdownTime)
                _browserHandler.PlainMain.PlaySound(_countdownSounds[_currentCountdownTime]);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettingsDto settings)
        {
            Stop();
        }
    }
}
