using GTANetworkAPI;
using GTANetworkMethods;
using Microsoft.AspNetCore.Connections.Features;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.LobbySystem.Vehicles
{
    public class GangLobbyVehicles : BaseLobbyVehicles
    {
        private readonly IGangLobby _lobby;
        private readonly IBaseLobbyEventsHandler _events;

        public GangLobbyVehicles(IGangLobby lobby, IBaseLobbyEventsHandler events)
        {
            _lobby = lobby;
            _events = events;

            events.PlayerJoinedAfter += Events_PlayerJoinedAfter;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby _)
        {
            if (_events.PlayerJoinedAfter is { })
                _events.PlayerJoinedAfter -= Events_PlayerJoinedAfter;
            _events.RemoveAfter -= RemoveEvents;
        }

        private void CreateGangVehicle(GangVehicles vehicleDb, IGang gang)
        {
            var vehicle = NAPI.Vehicle.CreateVehicle(vehicleDb.Model, new Vector3(vehicleDb.SpawnPosX, vehicleDb.SpawnPosY, vehicleDb.SpawnPosZ),
                       new Vector3(vehicleDb.SpawnRotX, vehicleDb.SpawnRotY, vehicleDb.SpawnRotZ),
                       vehicleDb.Color1, vehicleDb.Color2, alpha: 255, dimension: _lobby.MapHandler.Dimension) as ITDSVehicle;
            vehicle!.Lobby = _lobby;
            vehicle.Freeze(true);
            vehicle.SetInvincible(true);
            vehicle.SetCollisionsless(true);
            vehicle.SetGang(gang);
            //Todo: Disable collision of the vehicle (maybe on clientside an EntityStreamIn?)
        }

        private async ValueTask LoadGangVehiclesFirstTime(IGang gang)
        {
            if (gang.Entity.Vehicles is { })
                return;

            await gang.Database.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.Entry(gang.Entity).Collection(e => e.Vehicles).LoadAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            if (gang.Entity.Vehicles is null || gang.Entity.Vehicles.Count == 0)
                return;

            NAPI.Task.RunSafe(() =>
            {
                foreach (var vehicleDb in gang.Entity.Vehicles)
                    CreateGangVehicle(vehicleDb, gang);
            });
        }

        private async ValueTask Events_PlayerJoinedAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            if (!data.Player.Gang.Initialized)
            {
                await LoadGangVehiclesFirstTime(data.Player.Gang).ConfigureAwait(false);
                data.Player.Gang.Initialized = true;
            }
        }
    }
}
