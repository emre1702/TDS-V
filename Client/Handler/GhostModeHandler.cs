using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;

namespace TDS.Client.Handler
{
    public class GhostModeHandler : ServiceBase
    {
        #region Private Fields

        private TDSTimer _handleTimer;

        #endregion Private Fields

        #region Public Constructors

        public GhostModeHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        #endregion Public Constructors

        #region Private Methods

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

        private void Handle()
        {
            if (RAGE.Elements.Player.LocalPlayer.Vehicle is null)
                HandleWhileOnFoot();
            else
                HandleWhileInVehicle();
        }

        private void HandleWhileInVehicle()
        {
            var myVehicle = RAGE.Elements.Player.LocalPlayer.Vehicle;
            foreach (var vehicle in RAGE.Elements.Entities.Vehicles.Streamed)
            {
                if (vehicle == myVehicle)
                    continue;
                if (vehicle.IsSeatFree((int)VehicleSeat.DriverLeftFront - 1, 1))
                    continue;
                vehicle.SetNoCollisionEntity(myVehicle.Handle, true);
                //Todo: Is this enough or do I need to do it for localplayer vehicle, too?
            }

            foreach (var otherPlayer in RAGE.Elements.Entities.Players.Streamed)
            {
                if (otherPlayer == RAGE.Elements.Player.LocalPlayer)
                    continue;
                myVehicle.SetNoCollisionEntity(otherPlayer.Handle, true);
            }
        }

        private void HandleWhileOnFoot()
        {
            foreach (var vehicle in RAGE.Elements.Entities.Vehicles.Streamed)
            {
                if (vehicle.IsSeatFree((int)VehicleSeat.DriverLeftFront - 1, 1))
                    continue;
                vehicle.SetNoCollisionEntity(RAGE.Elements.Player.LocalPlayer.Handle, true);
                //Todo: Is this enough or do I need to do it for localplayer, too?
            }
        }

        #endregion Private Methods
    }
}
