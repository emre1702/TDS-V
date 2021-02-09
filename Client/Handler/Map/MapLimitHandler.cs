using RAGE;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Entities;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Client.Handler.Map
{
    public class MapLimitHandler : ServiceBase
    {
        private readonly DxHandler _dxHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private MapLimit _currentMapLimit;

        public MapLimitHandler(LoggingHandler loggingHandler, SettingsHandler settingsHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHander,
            DxHandler dxHandler, TimerHandler timerHandler)
            : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            eventsHander.InFightStatusChanged += EventsHander_InFightStatusChanged;
            eventsHander.MapBorderColorChanged += EventsHander_MapBorderColorChanged;
            eventsHander.LocalPlayerDied += Stop;
            eventsHander.MapCleared += Clear;
            eventsHander.Respawned += EventsHander_Respawned;
            eventsHander.RoundEnded += _ => Stop();
            eventsHander.RoundStarted += Start;
            eventsHander.LobbyLeft += Clear;
        }

        public void Load(List<Vector3> edges)
        {
            _currentMapLimit?.Stop();
            _currentMapLimit = new MapLimit(edges, _settingsHandler.MapLimitType, _settingsHandler.MapLimitTime, _settingsHandler.MapBorderColor, _remoteEventsSender, _settingsHandler,
                _dxHandler, _timerHandler);
        }

        public void Start(bool isSpectator)
        {
            if (!isSpectator)
                _currentMapLimit?.Start();
        }

        public void Stop()
        {
            _currentMapLimit?.Stop();
        }

        public void Clear()
        {
            Stop();
            _currentMapLimit?.SetEdges(null);
        }

        private void EventsHander_InFightStatusChanged(bool inFight)
        {
            if (inFight)
                _currentMapLimit?.SetType(_settingsHandler.MapLimitType, false);
            else
                _currentMapLimit?.SetType(TDS.Shared.Data.Enums.MapLimitType.Display, false);
        }

        private void EventsHander_MapBorderColorChanged(Color color)
        {
            if (!(_currentMapLimit is null))
                _currentMapLimit.MapBorderColor = color;
        }

        private void EventsHander_Respawned(bool inFightAgain)
        {
            Start(!inFightAgain);
        }

        private void Clear(SyncedLobbySettings settings)
        {
            Stop();
            _currentMapLimit = null;
        }
    }
}