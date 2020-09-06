using System;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Default;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS_Client.Handler.Lobby
{
    public class CountdownHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly string[] _countdownSounds = new string[] { "go", "1", "2", "3" };
        private readonly DxHandler _dxHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly LobbyCamHandler _lobbyCamHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private TDSTimer _countdownTimer;
        private int _currentCountdownTime;
        private DxText _text;

        public CountdownHandler(LoggingHandler loggingHandler, SettingsHandler settingsHandler, DxHandler dxHandler, TimerHandler timerHandler,
            BrowserHandler browserHandler, EventsHandler eventsHandler, LobbyCamHandler lobbyCamHandler)
            : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;
            _browserHandler = browserHandler;
            _eventsHandler = eventsHandler;
            _lobbyCamHandler = lobbyCamHandler;

            eventsHandler.LobbyLeft += _ => Stop();
            eventsHandler.MapCleared += Stop;
            eventsHandler.RoundStarted += _ => End();
            eventsHandler.RoundEnded += _ => Stop();

            RAGE.Events.Add(ToClientEvent.CountdownStart, OnCountdownStartMethod);
        }

        public void End()
        {
            try
            {
                _currentCountdownTime = 0;
                if (_countdownTimer != null)
                {
                    _countdownTimer?.Kill();
                    if (_text == null)
                        _text = new DxText(_dxHandler, _timerHandler, "GO", 0.5f, 0.2f, 2f, Color.White, Alignment: Alignment.Centered, alignmentY: AlignmentY.Center);
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
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Start()
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.Start");
                if (_settingsHandler.CountdownTime == 0)
                {
                    return;
                }
                _countdownTimer?.Kill();
                _currentCountdownTime = _settingsHandler.CountdownTime;
                _countdownTimer = new TDSTimer(Refresh, 1000, (uint)_currentCountdownTime + 1);
                _text = new DxText(_dxHandler, _timerHandler, _currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, Alignment: Alignment.Centered, alignmentY: AlignmentY.Center);
                Refresh();
                Logging.LogInfo("", "CountdownHandler.Start", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void StartAfterwards(int timeremainingms)
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.StartAfterwards");
                _currentCountdownTime = (int)Math.Ceiling((double)timeremainingms / 1000);
                _countdownTimer?.Kill();
                _countdownTimer = new TDSTimer(() =>
                {
                    if (_currentCountdownTime > 1)
                        _countdownTimer = new TDSTimer(Refresh, 1000, (uint)(_currentCountdownTime));
                    Refresh();
                }, (uint)(_currentCountdownTime - timeremainingms), 1);
                _text = new DxText(_dxHandler, _timerHandler, _currentCountdownTime.ToString(), 0.5f, 0.1f, 2f, Color.White, Alignment: Alignment.Centered, alignmentY: AlignmentY.Center);
                Refresh();
                Logging.LogInfo("", "CountdownHandler.StartAfterwards", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Stop()
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.Stop");
                _text?.Remove();
                _text = null;
                _countdownTimer?.Kill();
                _countdownTimer = null;
                Logging.LogInfo("", "CountdownHandler.Stop", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnCountdownStartMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.OnCountdownStartMethod");
                _eventsHandler.OnCountdownStarted((bool)args[0]);

                int mstimetoplayer = (int)Math.Ceiling(_settingsHandler.CountdownTime * 1000 * 0.9);
                if (args.Length <= 1)
                {
                    Start();
                    _lobbyCamHandler.SetGoTowardsPlayer(mstimetoplayer);
                }
                else
                {
                    int remainingms = (int)args[1];
                    StartAfterwards(remainingms);
                    int timeofcountdowncameraisatplayer = _settingsHandler.CountdownTime * 1000 - mstimetoplayer;
                    if (remainingms < timeofcountdowncameraisatplayer)
                        _lobbyCamHandler.SetGoTowardsPlayer(remainingms);
                    else
                        _lobbyCamHandler.SetGoTowardsPlayer(remainingms - timeofcountdowncameraisatplayer);
                }
                Logging.LogInfo("", "CountdownHandler.OnCountdownStartMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void PlaySound()
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.PlaySound");
                if (_countdownSounds.Length > _currentCountdownTime)
                    _browserHandler.PlainMain.PlaySound(_countdownSounds[_currentCountdownTime]);
                Logging.LogInfo("", "CountdownHandler.PlaySound", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void Refresh()
        {
            try
            {
                Logging.LogInfo("", "CountdownHandler.Refresh");
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
                Logging.LogInfo("", "CountdownHandler.Refresh", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}
