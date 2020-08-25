import { injectable, inject } from "inversify";
import DIIdentifier from "../../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../../events/events.service";
import LobbySettings from "../../../datas/interfaces/lobbies/lobby-settings.interface";
import LobbyType from "../../../datas/enums/lobbies/lobby-type.enum";
import alt from "alt-client";

@injectable()
export default class GangVehiclesService {

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService
    ) {
        eventsService.onLobbyJoined.on(this.onLobbyJoined.bind(this));
        eventsService.onLobbyLeft.on(this.onLobbyLeft.bind(this));
    }

    private start() {
        alt.on()
    }

    private stop() {

    }

    private onLobbyJoined(settings: LobbySettings) {
        if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
            return;
        this.start();
    }

    private onLobbyLeft(settings: LobbySettings) {
        if (settings.Type != LobbyType.GangLobby && !settings.IsGangActionLobby)
            return;
        this.stop();
    }
}

/*
 * using TDS_Client.Data.Interfaces.ModAPI;
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
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly EventMethodData<PlayerStartEnterVehicleDelegate> _vehicleStartEnterEventMethod;

        #endregion Private Fields


        #region Private Methods

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

        #endregion Private Methods
    }
}
*/
