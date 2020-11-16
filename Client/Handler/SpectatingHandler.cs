using RAGE.Elements;
using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Events;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class SpectatingHandler : ServiceBase
    {
        private static bool _binded;
        private static GameEntityBase _spectatingEntity;
        private readonly BindsHandler _bindsHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly DeathHandler _deathHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly UtilsHandler _utilsHandler;

        public SpectatingHandler(LoggingHandler loggingHandler, RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler,
            CamerasHandler camerasHandler, DeathHandler deathHandler,
            EventsHandler eventsHandler, UtilsHandler utilsHandler) : base(loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _bindsHandler = bindsHandler;
            _camerasHandler = camerasHandler;
            _deathHandler = deathHandler;
            _utilsHandler = utilsHandler;

            eventsHandler.LobbyLeft += _ => Stop();
            eventsHandler.CountdownStarted += EventsHandler_CountdownStarted;
            eventsHandler.RoundStarted += EventsHandler_RoundStarted;

            RAGE.Events.Add(ToClientEvent.SpectatorReattachCam, OnSpectatorReattachCamMethod);
            RAGE.Events.Add(ToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            RAGE.Events.Add(ToClientEvent.SetPlayerToSpectatePlayer, OnSetPlayerToSpectatePlayerMethod);
            RAGE.Events.Add(ToClientEvent.StopSpectator, OnStopSpectatorMethod);
        }

        public bool IsSpectator { get; set; }

        public GameEntityBase SpectatingEntity
        {
            get => _spectatingEntity;
            set
            {
                Logging.LogInfo("", "SpectatingHandler.SpectatingEntity");
                if (value == _spectatingEntity)
                    return;

                _spectatingEntity = value;

                if (value != null)
                {
                    _deathHandler.PlayerSpawn();
                    _camerasHandler.SpectateCam.Spectate(value);
                }
                else
                {
                    _camerasHandler.SpectateCam.Detach();
                }

                _camerasHandler.SpectateCam.Activate();
                _camerasHandler.SpectateCam.Render(true, Constants.DefaultSpectatePlayerChangeEaseTime);
                Logging.LogInfo("", "SpectatingHandler.SpectatingEntity", true);
            }
        }

        public void Start()
        {
            if (_binded)
                return;
            _binded = true;

            _deathHandler.PlayerSpawn();
            _camerasHandler.SpectateCam.Activate();
            _camerasHandler.SpectateCam.Render(true, Constants.DefaultSpectatePlayerChangeEaseTime);

            _bindsHandler.Add(Key.Right, Next);
            _bindsHandler.Add(Key.D, Next);
            _bindsHandler.Add(Key.Left, Previous);
            _bindsHandler.Add(Key.A, Previous);
        }

        public void Stop()
        {
            _spectatingEntity = null;
            if (!_binded)
                return;
            _binded = false;

            _camerasHandler.SpectateCam.Deactivate();

            _bindsHandler.Remove(Key.Right, Next);
            _bindsHandler.Remove(Key.D, Next);
            _bindsHandler.Remove(Key.Left, Previous);
            _bindsHandler.Remove(Key.A, Previous);

            IsSpectator = false;
        }

        private void EventsHandler_CountdownStarted(bool isSpectator)
        {
            if (isSpectator)
                Start();
        }

        private void EventsHandler_RoundStarted(bool isSpectator)
        {
            IsSpectator = isSpectator;
        }

        private void Next(Key _)
        {
            _remoteEventsSender.Send(ToServerEvent.SpectateNext, true);
        }

        private void OnPlayerSpectateModeMethod(object[] args)
        {
            IsSpectator = true;
            Start();
        }

        private void OnSetPlayerToSpectatePlayerMethod(object[] args)
        {
            var target = _utilsHandler.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            if (target != null)
            {
                SpectatingEntity = target;
            }
        }

        private void OnSpectatorReattachCamMethod(object[] args)
        {
            if (SpectatingEntity != null)
            {
                _camerasHandler.SpectateCam.Spectate(SpectatingEntity);
            }
        }

        private void OnStopSpectatorMethod(object[] args)
        {
            Stop();
        }

        private void Previous(Key _)
        {
            _remoteEventsSender.Send(ToServerEvent.SpectateNext, false);
        }
    }
}
