using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
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

        public MapLimitHandler(SettingsHandler settingsHandler, IModAPI modAPI, RemoteEventsSender remoteEventsSender, EventsHandler eventsHander)
        {
            _settingsHandler = settingsHandler;
            _modAPI = modAPI;
            _remoteEventsSender = remoteEventsSender;

            eventsHander.InFightStatusChanged += EventsHander_InFightStatusChanged;
        }

        public void Load(List<Position3D> edges)
        {
            _currentMapLimit?.Stop();
            _currentMapLimit = new MapLimit(edges, _settingsHandler.MapLimitType, _settingsHandler.MapLimitTime, _settingsHandler.MapBorderColor, _modAPI, _remoteEventsSender, _settingsHandler);
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
