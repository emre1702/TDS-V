using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.GangSystem
{
    public class GangVehiclesHandler : ServiceBase
    {
        private readonly EventMethodData<PlayerStartEnterVehicleDelegate> _vehicleStartEnterEventMethod;

        private readonly DataSyncHandler _dataSyncHandler;

        public GangVehiclesHandler(IModAPI modAPI, LoggingHandler loggingHandler, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            _dataSyncHandler = dataSyncHandler;

            _vehicleStartEnterEventMethod = new EventMethodData<PlayerStartEnterVehicleDelegate>(HandleDriverOnlyGangMembers);

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        private void HandleDriverOnlyGangMembers(IVehicle vehicle, VehicleSeat seat, CancelEventArgs cancel)
        {
            //Todo: Prevent stealing other vehicles when gang Id -1 if it's not mine
            if (seat == VehicleSeat.DriverLeftFront
                && _dataSyncHandler.GetData(vehicle, EntityDataKey.GangId, -2) != _dataSyncHandler.GetData(PlayerDataKey.GangId, -1))
                cancel.Cancel = true;
        }

        private void Start()
        {
            ModAPI.Event.PlayerStartEnterVehicle.Add(_vehicleStartEnterEventMethod);
        }

        private void Stop()
        {
            ModAPI.Event.PlayerStartEnterVehicle.Remove(_vehicleStartEnterEventMethod);
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
                return;
            Start();
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
                return;
            Stop();
        }
    }
}
