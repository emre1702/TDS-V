using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Freeroam;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Freeroam
{
    public class FreeroamLobbyFreeroam : IFreeroamLobbyFreeroam
    {
        private readonly FreeroamDataHandler _freeroamDataHandler;
        private readonly IBaseLobby _lobby;
        private readonly IBaseLobbyEventsHandler _eventsHandler;
        private readonly RemoteBrowserEventsHandler _remoteBrowserEventsHandler;

        public FreeroamLobbyFreeroam(IBaseLobby lobby, FreeroamDataHandler freeroamDataHandler, IBaseLobbyEventsHandler eventsHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            (_lobby, _freeroamDataHandler, _eventsHandler, _remoteBrowserEventsHandler) = (lobby, freeroamDataHandler, eventsHandler, remoteBrowserEventsHandler);

            _eventsHandler.RemoveAfter += RemoveEvents;

            remoteBrowserEventsHandler.Add(ToServerEvent.GetVehicle, GiveVehicle, player => player.Lobby == _lobby);
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            _eventsHandler.RemoveAfter -= RemoveEvents;

            _remoteBrowserEventsHandler.Remove(ToServerEvent.GetVehicle, GiveVehicle);
        }

        public void GiveVehicle(ITDSPlayer player, FreeroamVehicleType vehType)
        {
            var vehicleHash = _freeroamDataHandler.GetDefaultHash(vehType);
            if (!vehicleHash.HasValue)
                return;

            NAPI.Task.RunSafe(() =>
            {
                var pos = player.Position;
                if (player.FreeroamVehicle is { })
                {
                    if (player.IsInVehicle)
                        player.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                var vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash.Value, pos, player.Rotation.Z, 0, 0, player.Name, dimension: _lobby.MapHandler.Dimension) as ITDSVehicle;
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle!, true);

                player.SetIntoVehicle(vehicle, 0);
            });
        }

        private object? GiveVehicle(RemoteBrowserEventArgs args)
        {
            if (args.Args.Count == 0)
                return null;

            if (!Enum.TryParse(args.Args[0].ToString(), out FreeroamVehicleType vehType))
                return null;

            GiveVehicle(args.Player, vehType);
            return null;
        }

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.Position = new Vector3(x, y, z);
            player.Rotation = new Vector3(0, 0, rot);
        }
    }
}