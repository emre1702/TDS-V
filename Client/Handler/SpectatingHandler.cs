using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class SpectatingHandler
    {
        public bool IsSpectator { get; set; }

        public IEntityBase SpectatingEntity
        {
            get => _spectatingEntity;
            set
            {
                if (value == _spectatingEntity)
                    return;

                _spectatingEntity = value;

                if (value != null)
                    _camerasHandler.SpectateCam.Spectate(value);

                _camerasHandler.SpectateCam.Render(true, Constants.DefaultSpectatePlayerChangeEaseTime);
            }
        }


        private static IEntityBase _spectatingEntity;
        private static bool _binded;

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly BindsHandler _bindsHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly DeathHandler _deathHandler;
        private readonly UtilsHandler _utilsHandler;

        public SpectatingHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler, CamerasHandler camerasHandler, DeathHandler deathHandler, 
            EventsHandler eventsHandler, UtilsHandler utilsHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _bindsHandler = bindsHandler;
            _camerasHandler = camerasHandler;
            _deathHandler = deathHandler;
            _utilsHandler = utilsHandler;

            eventsHandler.LobbyLeft += _ => Stop();
            eventsHandler.MapCleared += Stop;
            eventsHandler.CountdownStarted += EventsHandler_CountdownStarted;
            eventsHandler.RoundStarted += EventsHandler_RoundStarted;

            modAPI.Event.Add(ToClientEvent.SpectatorReattachCam, OnSpectatorReattachCamMethod);
            modAPI.Event.Add(ToClientEvent.PlayerSpectateMode, OnPlayerSpectateModeMethod);
            modAPI.Event.Add(ToClientEvent.SetPlayerToSpectatePlayer, OnSetPlayerToSpectatePlayerMethod);
            modAPI.Event.Add(ToClientEvent.StopSpectator, OnStopSpectatorMethod);
        }


        private void Next(Key _)
        {
            _remoteEventsSender.Send(ToServerEvent.SpectateNext, true);
        }

        private void Previous(Key _)
        {
            _remoteEventsSender.Send(ToServerEvent.SpectateNext, false);
        }

        public void Start()
        {
            if (_binded)
                return;
            _binded = true;

            _deathHandler.PlayerSpawn();
            _camerasHandler.SpectateCam.Activate();

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

        private void OnPlayerSpectateModeMethod(object[] args)
        {
            IsSpectator = true;
            Start();
        }

        private void EventsHandler_RoundStarted(bool isSpectator)
        {
            IsSpectator = isSpectator;
        }

        private void OnSetPlayerToSpectatePlayerMethod(object[] args)
        {
            IPlayer target = _utilsHandler.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            if (target != null)
                SpectatingEntity = target;
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

    }
}
