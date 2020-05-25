using System;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class AFKCheckHandler : ServiceBase
    {
        #region Private Fields

        private readonly DxHandler _dxHandler;
        private readonly EventMethodData<TickDelegate> _onTickEventMethod;
        private readonly PlayerFightHandler _playerFightHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private readonly EventMethodData<WeaponShotDelegate> _weaponShotMethod;
        private Position3D _afkStartPos;
        private TDSTimer _checkTimer;
        private DxTextRectangle _draw;
        private bool _inAFKCheckLobby;
        private TDSTimer _kickTimer;
        private Position3D _lastPos;

        #endregion Private Fields

        #region Public Constructors

        public AFKCheckHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler,
            RemoteEventsSender remoteEventsSender, PlayerFightHandler playerFightHandler, TimerHandler timerHandler, DxHandler dxHandler)
            : base(modAPI, loggingHandler)
        {
            _onTickEventMethod = new EventMethodData<TickDelegate>(OnTick);
            _weaponShotMethod = new EventMethodData<WeaponShotDelegate>(Event_WeaponShot);

            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _playerFightHandler = playerFightHandler;
            _timerHandler = timerHandler;
            _dxHandler = dxHandler;

            eventsHandler.LobbyJoined += OnLobbyJoin;
            eventsHandler.LobbyLeft += OnLobbyLeave;
            eventsHandler.LocalPlayerDied += OnDeath;
            eventsHandler.RoundStarted += OnRoundStart;
            eventsHandler.RoundEnded += OnRoundEnded;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnDeath()
        {
            if (!(_kickTimer is null) && (int)(_kickTimer.RemainingMsToExecute) <= _settingsHandler.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
                IsAFKEnd();
        }

        public void OnRoundEnded(bool _)
        {
            if (_checkTimer is null)
                return;
            StopCheck();
        }

        public void OnRoundStart(bool isSpectator)
        {
            if (!_inAFKCheckLobby)
                return;
            if (!_settingsHandler.PlayerSettings.CheckAFK)
                return;
            if (isSpectator)
                return;
            _lastPos = ModAPI.LocalPlayer.Position;
            _checkTimer = new TDSTimer(Check, 5 * 1000, 0);
            if (!ModAPI.Event.WeaponShot.Contains(_weaponShotMethod))
                ModAPI.Event.WeaponShot.Add(_weaponShotMethod);
        }

        public void OnShoot()
        {
            StopAFK();
        }

        #endregion Public Methods

        #region Private Methods

        private bool CanBeAFK()
        {
            return _playerFightHandler.InFight
                && _settingsHandler.PlayerSettings.CheckAFK
                && ModAPI.LocalPlayer.IsPlaying()
                && !ModAPI.LocalPlayer.IsClimbing()
                && !ModAPI.LocalPlayer.IsFreeAiming();
        }

        private void Check()
        {
            if (!CanBeAFK())
            {
                StopAFK();
                return;
            }

            var currentPos = ModAPI.LocalPlayer.Position;
            if (_kickTimer is null)
            {
                var previousPos = _lastPos;
                _lastPos = currentPos;
                if (currentPos.DistanceTo(previousPos) > Constants.NeededDistanceToBeNotAFK)
                    return;

                IsAFKStart();
            }
            else
            {
                if (currentPos.DistanceTo(_afkStartPos) <= Constants.NeededDistanceToBeNotAFK)
                    return;

                StopAFK();
            }
        }

        private void Event_WeaponShot(Position3D targetPos, IPlayer target, CancelEventArgs cancel)
        {
            OnShoot();
        }

        private string GetWarning()
        {
            int secsLeft = (int)Math.Ceiling((double)(_kickTimer.RemainingMsToExecute / 1000));
            return string.Format(_settingsHandler.Language.AFK_KICK_WARNING, secsLeft);
        }

        private bool IsAFKCheckLobby(SyncedLobbySettings settings)
        {
            return settings?.Type == LobbyType.Arena && settings?.IsOfficial == true;
        }

        private void IsAFKEnd()
        {
            _afkStartPos = null;
            StopCheck();
            ModAPI.Chat.Output(_settingsHandler.Language.AFK_KICK_INFO);
            _remoteEventsSender.Send(ToServerEvent.LeaveLobby);
        }

        private void IsAFKStart()
        {
            _afkStartPos = ModAPI.LocalPlayer.Position;
            ModAPI.Event.Tick.Add(_onTickEventMethod);
            _kickTimer = new TDSTimer(IsAFKEnd, (uint)_settingsHandler.PlayerSettings.AFKKickAfterSeconds * 1000, 1);
        }

        private bool IsStillAFK()
        {
            if (!CanBeAFK())
                return false;

            Position3D currentPos = ModAPI.LocalPlayer.Position;
            if (currentPos.DistanceTo(_afkStartPos) > Constants.NeededDistanceToBeNotAFK)
                return false;

            return true;
        }

        private void OnLobbyJoin(SyncedLobbySettings settings)
        {
            _inAFKCheckLobby = IsAFKCheckLobby(settings);
            _lastPos = null;
        }

        private void OnLobbyLeave(SyncedLobbySettings settings)
        {
            if (!IsAFKCheckLobby(settings))
                return;

            StopCheck();
        }

        private void OnTick(int currentTick)
        {
            if (!IsStillAFK())
            {
                StopAFK();
                return;
            }

            if ((int)_kickTimer.RemainingMsToExecute > _settingsHandler.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
                return;

            if (_draw is null)
            {
                _draw = new DxTextRectangle(_dxHandler, ModAPI, _timerHandler, GetWarning().ToString(), 0, 0, 1, 1, Color.FromArgb(255, 255, 255), Color.FromArgb(40, 200, 0, 0), 1.2f,
                    frontPriority: 1, relativePos: true);
            }
            else
            {
                _draw.SetText(GetWarning().ToString());
            }
        }

        private void StopAFK()
        {
            if (_kickTimer is null)
                return;
            _kickTimer.Kill();
            _kickTimer = null;
            ModAPI.Event.Tick.Remove(_onTickEventMethod);
            _draw?.Remove();
            _draw = null;
        }

        private void StopCheck()
        {
            _checkTimer?.Kill();
            _checkTimer = null;
            StopAFK();
            ModAPI.Event.WeaponShot.Remove(_weaponShotMethod);
        }

        #endregion Private Methods
    }
}
