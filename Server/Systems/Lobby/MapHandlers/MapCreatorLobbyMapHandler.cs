using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.Map.Creator;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class MapCreatorLobbyMapHandler : BaseLobbyMapHandler, IMapCreatorMapHandler
    {
        private LocationData? _currentLocation;

        public MapCreatorLobbyMapHandler(IMapCreatorLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                if (_currentLocation is { })
                    data.Player.Spawn(_currentLocation.Position.ToVector3());
                else
                    data.Player.Spawn(SpawnPoint, SpawnRotation);
                data.Player.Freeze(false);
            });
            return default;
        }

        public void SyncLocation(ITDSPlayer player, string json)
        {
            _currentLocation = !string.IsNullOrEmpty(json) ? Serializer.FromBrowser<LocationData>(json) : null;

            if (_currentLocation is { })
            {
                var pos = _currentLocation.Position.ToVector3();
                Lobby.Players.DoInMain(p =>
                {
                    p.Spawn(pos.Around(1f, false));
                    if (p != player)
                        p.TriggerEvent(ToClientEvent.MapCreatorChangeLocation, json);
                });
            }
        }
    }
}