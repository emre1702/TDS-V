using RAGE.Elements;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using static RAGE.Events;

namespace TDS_Client.Handler.GangSystem
{
    public class GangVehiclesHandler : ServiceBase
    {
        private readonly DataSyncHandler _dataSyncHandler;

        public GangVehiclesHandler(LoggingHandler loggingHandler, DataSyncHandler dataSyncHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            _dataSyncHandler = dataSyncHandler;

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
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

        private void HandleDriverOnlyGangMembers(Vehicle vehicle, int seatId, CancelEventArgs cancel)
        {
            //Todo: Prevent stealing other vehicles when gang Id -1 if it's not mine
            if (seatId == (int)VehicleSeat.DriverLeftFront
                && _dataSyncHandler.GetData(vehicle, EntityDataKey.GangId, -2) != _dataSyncHandler.GetData(PlayerDataKey.GangId, -1))
                cancel.Cancel = true;
        }

        private void Start()
        {
            OnPlayerStartEnterVehicle += HandleDriverOnlyGangMembers;
        }

        private void Stop()
        {
            OnPlayerStartEnterVehicle -= HandleDriverOnlyGangMembers;
        }
    }
}
