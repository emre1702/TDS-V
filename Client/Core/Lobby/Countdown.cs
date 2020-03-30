using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;

namespace TDS_Client.Manager.Lobby
{
    internal static class Countdown
    {
        private static DxText _text;
        private static TDSTimer _countdownTimer;
        private static int _currentCountdownTime;

        private static readonly string[] _countdownSounds = new string[] { "go", "1", "2", "3" };

        public static void Start()
        {
            if (Settings.CountdownTime == 0)
            {
                return;
            }
            _countdownTimer?.Kill();
            _currentCountdownTime = Settings.CountdownTime;
            _countdownTimer = new TDSTimer(Refresh, 1000, (uint)_currentCountdownTime + 1);
            _text = new DxText(_currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, alignmentX: AlignmentX.Center, ToServerEvent: AlignmentY.Center);
            Refresh();
        }

        public static void StartAfterwards(int timeremainingms)
        {
            _currentCountdownTime = (int)Math.Ceiling((double)timeremainingms / 1000);
            _countdownTimer?.Kill();
            _countdownTimer = new TDSTimer(() =>
            {
                if (_currentCountdownTime > 1)
                    _countdownTimer = new TDSTimer(Refresh, 1000, (uint)(_currentCountdownTime));
                Refresh();
            }, (uint)(_currentCountdownTime - timeremainingms), 1);
            _text = new DxText(_currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, alignmentX: UIResText.Alignment.Centered, ToServerEvent: AlignmentY.Center);
            Refresh();
        }

        public static void End(bool showGo = true)
        {
            _currentCountdownTime = 0;
            if (showGo)
            {
                _countdownTimer?.Kill();
                if (_text == null)
                    _text = new DxText("GO", 0.5f, 0.2f, 2f, Color.White, alignmentX: UIResText.Alignment.Centered, ToServerEvent: AlignmentY.Center);
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

        public static void Stop()
        {
            _text?.Remove();
            _text = null;
            _countdownTimer?.Kill();
            _countdownTimer = null;
        }

        private static void Refresh()
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

        private static void PlaySound()
        {
            if (_countdownSounds.Length > _currentCountdownTime)
                MainBrowser.PlaySound(_countdownSounds[_currentCountdownTime]);
        }
    }
}
