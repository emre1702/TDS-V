using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class GhostModeHandler : ServiceBase
    {
        private TDSTimer _handleTimer;

        public GhostModeHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }


        private void Handle()
        {
            if (ModAPI.LocalPlayer.Vehicle is null)
                HandleWhileOnFoot();
            else
                HandleWhileInVehicle();
        }

        private void HandleWhileInVehicle()
        {
            var myVehicle = ModAPI.LocalPlayer.Vehicle;
            foreach (var vehicle in ModAPI.Pool.Vehicles.Streamed)
            {
                if (vehicle == myVehicle)
                    continue;
                if (vehicle.IsSeatFree(VehicleSeat.DriverLeftFront))
                    continue;
                vehicle.SetNoCollisionEntity(myVehicle.Handle);
                //Todo: Is this enough or do I need to do it for localplayer vehicle, too?
            }

            foreach (var otherPlayer in ModAPI.Pool.Players.Streamed)
            {
                if (otherPlayer == ModAPI.LocalPlayer)
                    continue;
                myVehicle.SetNoCollisionEntity(otherPlayer.Handle);
            }
        }

        private void HandleWhileOnFoot()
        {
            foreach (var vehicle in ModAPI.Pool.Vehicles.Streamed)
            {
                if (vehicle.IsSeatFree(VehicleSeat.DriverLeftFront))
                    continue;
                vehicle.SetNoCollisionEntity(ModAPI.LocalPlayer.Handle);
                //Todo: Is this enough or do I need to do it for localplayer, too?
            }
        }


        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
                return;

            _handleTimer?.Kill();
            _handleTimer = new TDSTimer(Handle, 1000, 0);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
                return;

            _handleTimer?.Kill();
            _handleTimer = null;
        }
    }
}
