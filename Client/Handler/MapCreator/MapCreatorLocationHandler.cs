using RAGE.Game;
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

        public MapCreatorLocationHandler(RemoteEventsSender remoteEventsSender)
        {
            _remoteEventsSender = remoteEventsSender;

            RAGE.Events.Add(FromBrowserEvent.MapCreatorChangeLocation, ChangeLocationFromBrowser);
            RAGE.Events.Add(ToClientEvent.MapCreatorChangeLocation, ChangeLocationFromServer);
        }

        private void ChangeLocationFromBrowser(object[] args)
        {
            var json = args.Length > 0 ? Convert.ToString(args[0]) : "";
            var location = json.Length > 0 ? Serializer.FromBrowser<LocationData>(json) : null;
            ChangeLocation(location);
            _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.MapCreatorSyncLocation, json);
        }

        private void ChangeLocationFromServer(object[] args)
        {
            var location = Serializer.FromBrowser<LocationData>(Convert.ToString(args[0]));
            ChangeLocation(location);
        }

        public void ChangeLocation(IMapLocationData newLocation)
        {
            _currentLocationData?.UnloadLocation();
            _currentLocationData = newLocation;
            _currentLocationData?.LoadLocation();
        }
    }
}