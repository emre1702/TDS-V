using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class SpectatingHandler
    {
        public bool IsSpectator { get; set; }

        public IEntity SpectatingEntity
        {
            get => _spectatingEntity;
            set
            {
                if (value == _spectatingEntity)
                    return;
                if (_spectatingEntity == null)
                    Start();
                else if (value == null)
                    Stop();

                _spectatingEntity = value;

                if (value != null)
                    _camerasHandler.SpectateCam.Spectate(value);

                _camerasHandler.SpectateCam.Render(true, Constants.DefaultSpectatePlayerChangeEaseTime);
            }
        }


        private static IEntity _spectatingEntity;
        private static bool _binded;

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly BindsHandler _bindsHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly DeathHandler _deathHandler;

        public SpectatingHandler(RemoteEventsSender remoteEventsSender, BindsHandler bindsHandler, CamerasHandler camerasHandler, DeathHandler deathHandler, EventsHandler eventsHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _bindsHandler = bindsHandler;
            _camerasHandler = camerasHandler;
            _deathHandler = deathHandler;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
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

            _bindsHandler.Add(Key.RightArrow, Next);
            _bindsHandler.Add(Key.D, Next);
            _bindsHandler.Add(Key.LeftArrow, Previous);
            _bindsHandler.Add(Key.A, Previous);
        }

        public void Stop()
        {
            if (!_binded)
                return;
            _binded = false;

            _camerasHandler.SpectateCam.Deactivate();

            _bindsHandler.Remove(Key.RightArrow, Next);
            _bindsHandler.Remove(Key.D, Next);
            _bindsHandler.Remove(Key.LeftArrow, Previous);
            _bindsHandler.Remove(Key.A, Previous);

            IsSpectator = false;
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettingsDto settings)
        {
            SpectatingEntity = null;
        }
    }
}
