using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Interfaces.Map.Creator;
using TDS.Shared.Data.Models.Map.Creator;
using TDS.Shared.Default;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorLocationHandler
    {
        private IMapLocationData _currentLocationData;

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly LoggingHandler _loggingHandler;

        public MapCreatorLocationHandler(RemoteEventsSender remoteEventsSender, LoggingHandler loggingHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _loggingHandler = loggingHandler;

            RAGE.Events.Add(FromBrowserEvent.MapCreatorChangeLocation, ChangeLocationFromBrowser);
            RAGE.Events.Add(ToClientEvent.MapCreatorChangeLocation, ChangeLocationFromServer);
        }

        public void Stop()
        {
            try
            {
                _currentLocationData?.UnloadLocation();
                _currentLocationData = null;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private void ChangeLocationFromBrowser(object[] args)
        {
            try
            {
                var json = args.Length > 0 ? Convert.ToString(args[0]) : "";
                var location = json.Length > 0 ? Serializer.FromBrowser<LocationData>(json) : null;
                ChangeLocation(location);
                _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncLocation, json);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private void ChangeLocationFromServer(object[] args)
        {
            try
            {
                var location = Serializer.FromBrowser<LocationData>(Convert.ToString(args[0]));
                ChangeLocation(location);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void ChangeLocation(IMapLocationData newLocation)
        {
            try
            {
                _currentLocationData?.UnloadLocation();
                _currentLocationData = newLocation;
                _currentLocationData?.LoadLocation();
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}