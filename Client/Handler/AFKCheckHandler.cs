using System;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Entities.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class AFKCheckHandler
    {
        private Position3D _afkStartPos;
        private Position3D _lastPos;
        private TDSTimer _checkTimer;
        private TDSTimer _kickTimer;
        private bool _inAFKCheckLobby;

        private DxTextRectangle _draw;

        private readonly EventMethodData<TickDelegate> _onTickEventMethod;

        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly PlayerFightHandler _playerFightHandler;

        public AFKCheckHandler(EventsHandler eventsHandler, IModAPI modAPI, SettingsHandler settingsHandler, RemoteEventsSender remoteEventsSender, PlayerFightHandler playerFightHandler)
        {
            _onTickEventMethod = new EventMethodData<TickDelegate>(OnTick);

            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _playerFightHandler = playerFightHandler;

            eventsHandler.LobbyJoin += OnLobbyJoin;
            eventsHandler.LobbyLeave += OnLobbyLeave;
            eventsHandler.Death += OnDeath;
            eventsHandler.RoundStart += OnRoundStart;
            eventsHandler.RoundEnd += OnRoundEnd;
        }

        private void Check()
        {
            if (!CanBeAFK())
            {
                StopAFK();
                return;
            }

            var currentPos = _modAPI.LocalPlayer.Position;
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

        private void OnTick(ulong currentTick)
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
                _draw = new DxTextRectangle(GetWarning().ToString(), 0, 0, 1, 1, Color.FromArgb(255, 255, 255), Color.FromArgb(40, 200, 0, 0), 1.2f,
                    frontPriority: 1, relativePos: true);
            }
            else
            {
                _draw.SetText(GetWarning().ToString());
            }

        }

        private bool CanBeAFK()
        {
            return _playerFightHandler.InFight
                && _settingsHandler.PlayerSettings.CheckAFK
                && _modAPI.LocalPlayer.IsPlaying
                && !_modAPI.LocalPlayer.IsClimbing
                && !_modAPI.LocalPlayer.IsFreeAiming;
        }

        private bool IsStillAFK()
        {
            if (!CanBeAFK())
                return false;

            Position3D currentPos = _modAPI.LocalPlayer.Position;
            if (currentPos.DistanceTo(_afkStartPos) > Constants.NeededDistanceToBeNotAFK)
                return false;

            return true;
        }

        public void OnRoundStart(bool isSpectator)
        {
            if (!_inAFKCheckLobby)
                return;
            if (isSpectator)
                return;
            _lastPos = _modAPI.LocalPlayer.Position;
            _checkTimer = new TDSTimer(Check, 5 * 1000, 0);
        }

        public void OnRoundEnd()
        {
            if (_checkTimer is null)
                return;
            StopCheck();
        }

        private void OnLobbyJoin(SyncedLobbySettingsDto settings)
        {
            _inAFKCheckLobby = IsAFKCheckLobby(settings);
            _lastPos = null;

        }

        private void OnLobbyLeave(SyncedLobbySettingsDto settings)
        {
            if (!IsAFKCheckLobby(settings))
                return;

            StopCheck();
        }

        public void OnShoot()
        {
            StopAFK();
        }

        public void OnDeath()
        {
            if (!(_kickTimer is null) && (int)(_kickTimer.RemainingMsToExecute) <= _settingsHandler.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
                IsAFKEnd();
        }

        private void IsAFKStart()
        {
            _afkStartPos = _modAPI.LocalPlayer.Position;
            _modAPI.Event.Tick.Add(_onTickEventMethod);
            _kickTimer = new TDSTimer(IsAFKEnd, (uint)_settingsHandler.PlayerSettings.AFKKickAfterSeconds * 1000, 1);
        }

        private void IsAFKEnd()
        {
            _afkStartPos = null;
            StopCheck();
            _modAPI.Chat.Output(_settingsHandler.Language.AFK_KICK_INFO);
            _remoteEventsSender.Send(ToServerEvent.LeaveLobby);
        }

        private void StopCheck()
        {
            _checkTimer?.Kill();
            _checkTimer = null;
            StopAFK();
        }

        private void StopAFK()
        {
            if (_kickTimer is null)
                return;
            _kickTimer.Kill();
            _kickTimer = null;
            _modAPI.Event.Tick.Remove(_onTickEventMethod);
            _draw?.Remove();
            _draw = null;
        }

        private bool IsAFKCheckLobby(SyncedLobbySettingsDto settings)
        {
            return settings?.Type == LobbyType.Arena && settings?.IsOfficial == true;
        }

        private string GetWarning()
        {
            int secsLeft = (int)Math.Ceiling((double)(_kickTimer.RemainingMsToExecute / 1000));
            return string.Format(_settingsHandler.Language.AFK_KICK_WARNING, secsLeft);
        }
    }
}
