using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Entities;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Map
{
    public class MapLimitHandler
    {
        private MapLimit _currentMapLimit;

        private readonly SettingsHandler _settingsHandler;
        private readonly IModAPI _modAPI;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly DxHandler _dxHandler;
        private readonly TimerHandler _timerHandler;

        public MapLimitHandler(SettingsHandler settingsHandler, IModAPI modAPI, RemoteEventsSender remoteEventsSender, EventsHandler eventsHander, DxHandler dxHandler, TimerHandler timerHandler)
        {
            _settingsHandler = settingsHandler;
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;
            _dxHandler = dxHandler;
            _timerHandler = timerHandler;

            eventsHander.InFightStatusChanged += EventsHander_InFightStatusChanged;
        }

        public void Load(List<Position3D> edges)
        {
            _currentMapLimit?.Stop();
            _currentMapLimit = new MapLimit(edges, _settingsHandler.MapLimitType, _settingsHandler.MapLimitTime, _settingsHandler.MapBorderColor, _modAPI, _remoteEventsSender, _settingsHandler, 
                _dxHandler, _timerHandler);
        }

        public void Start()
        {
            _currentMapLimit?.Start();
        }

        public void Stop()
        {
            _currentMapLimit?.Stop();
        }

        private void EventsHander_InFightStatusChanged(bool inFight)
        {
            if (inFight)
                Start();
            else
                Stop();
        }
    }
}
