using RAGE;
using RAGE.Game;
using System;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Lobby;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Common.Instance.Utility;
using player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    static class AFKCheckManager
    {
        private static Vector3 _afkStartPos;
        private static Vector3 _lastPos;
        private static TDSTimer _checkTimer;
        private static TDSTimer _kickTimer;
        private static bool _inAFKCheckLobby;

        private static DxTextRectangle _draw;

        public static void Init()
        {
            CustomEventManager.OnLobbyJoin += OnLobbyJoin;
            CustomEventManager.OnLobbyLeave += OnLobbyLeave;
            CustomEventManager.OnDeath += OnDeath;
            CustomEventManager.OnRoundStart += OnRoundStart;
            CustomEventManager.OnRoundEnd += OnRoundEnd;
        }

        private static void Check()
        {
            if (!CanBeAFK())
            {
                StopAFK();
                return;
            }

            var currentPos = player.LocalPlayer.Position;
            if (_kickTimer is null)
            {
                var previousPos = _lastPos;
                _lastPos = currentPos;
                if (currentPos.DistanceTo(previousPos) > ClientConstants.NeededDistanceToBeNotAFK)
                    return;

                IsAFKStart();
            }
            else
            {
                if (currentPos.DistanceTo(_afkStartPos) <= ClientConstants.NeededDistanceToBeNotAFK)
                    return;

                StopAFK();
            }
        }

        private static void OnTick()
        {
            if (!IsStillAFK())
            {
                StopAFK();
                return;
            }

            if ((int)_kickTimer.RemainingMsToExecute > Settings.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
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

        private static bool CanBeAFK()
        {
            return Round.InFight && Settings.PlayerSettings.CheckAFK && Player.IsPlayerPlaying();
        }

        private static bool IsStillAFK()
        {
            if (!CanBeAFK())
                return false;
                
            Vector3 currentPos = player.LocalPlayer.Position;
            if (currentPos.DistanceTo(_afkStartPos) > ClientConstants.NeededDistanceToBeNotAFK)
                return false;

            return true;
        }

        public static void OnRoundStart(bool isSpectator)
        {
            if (!_inAFKCheckLobby)
                return;
            if (isSpectator)
                return;
            _lastPos = player.LocalPlayer.Position;
            _checkTimer = new TDSTimer(Check, 5 * 1000, 0);
        }

        public static void OnRoundEnd()
        {
            if (_checkTimer is null)
                return;
            StopCheck();
        }

        private static void OnLobbyJoin(SyncedLobbySettingsDto settings)
        {
            _inAFKCheckLobby = IsAFKCheckLobby(settings);
            _lastPos = null;
            
        }

        private static void OnLobbyLeave(SyncedLobbySettingsDto settings)
        {
            if (!IsAFKCheckLobby(settings))
                return;

            StopCheck();
        }

        public static void OnShoot()
        {
            StopAFK();
        }

        public static void OnDeath()
        {
            if (!(_kickTimer is null) && (int)(_kickTimer.RemainingMsToExecute) <= Settings.PlayerSettings.AFKKickShowWarningLastSeconds * 1000)
                IsAFKEnd();
        }

        private static void IsAFKStart()
        {
            _afkStartPos = player.LocalPlayer.Position;
            TickManager.Add(OnTick);
            _kickTimer = new TDSTimer(IsAFKEnd, (uint)Settings.PlayerSettings.AFKKickAfterSeconds * 1000, 1);
        }

        private static void IsAFKEnd()
        {
            _afkStartPos = null;
            StopCheck();
            Chat.Output(Settings.Language.AFK_KICK_INFO);
            EventsSender.Send(DToServerEvent.LeaveLobby);
        }

        private static void StopCheck()
        {
            _checkTimer?.Kill();
            _checkTimer = null;
            StopAFK();
        }

        private static void StopAFK()
        {
            if (_kickTimer is null)
                return;
            _kickTimer.Kill();
            _kickTimer = null;
            TickManager.Remove(OnTick);
            _draw?.Remove();
            _draw = null;
        }

        private static bool IsAFKCheckLobby(SyncedLobbySettingsDto settings)
        {
            return settings?.Type == ELobbyType.Arena && settings?.IsOfficial == true;
        }

        private static string GetWarning()
        {
            int secsLeft = (int)Math.Ceiling((double)(_kickTimer.RemainingMsToExecute / 1000));
            return string.Format(Settings.Language.AFK_KICK_WARNING, secsLeft);
        }
    }
}
