using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Map
{
    public class MapLimitHandler : ServiceBase
    {
        #region Private Fields

        private readonly DxHandler _dxHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;
        private readonly TimerHandler _timerHandler;
        private MapLimit _currentMapLimit;

        #endregion Private Fields

        #region Public Constructors

        public MapLimitHandler(IModAPI modAPI, LoggingHandler loggingHandler, SettingsHandler settingsHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHander,
            DxHandler dxHandler, TimerHandler timerHandler)
            : base(modAPI, loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _remoteEventsSender = remoteEventsSender;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            eventsHander.InFightStatusChanged += EventsHander_InFightStatusChanged;
            eventsHander.MapBorderColorChanged += EventsHander_MapBorderColorChanged;
            eventsHander.LobbyLeft += _ => Stop();
            eventsHander.LocalPlayerDied += Stop;
            eventsHander.MapCleared += Stop;
            eventsHander.Respawned += EventsHander_Respawned;
            eventsHander.RoundEnded += _ => Stop();
            eventsHander.RoundStarted += Start;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(List<Position> edges)
        {
            _currentMapLimit?.Stop();
            _currentMapLimit = new MapLimit(edges, _settingsHandler.MapLimitType, _settingsHandler.MapLimitTime, _settingsHandler.MapBorderColor, ModAPI, _remoteEventsSender, _settingsHandler,
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

        #endregion Public Methods

        #region Private Methods

        private void EventsHander_InFightStatusChanged(bool inFight)
        {
            if (inFight)
                _currentMapLimit.SetType(_settingsHandler.MapLimitType, false);
            else
                _currentMapLimit.SetType(TDS_Shared.Data.Enums.MapLimitType.Display, false);
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

        #endregion Private Methods
    }
}
