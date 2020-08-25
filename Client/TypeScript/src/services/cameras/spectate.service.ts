import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import alt from "alt-client";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";

@injectable()
export default class SpectateService {

    private binded: boolean;

    constructor(
        @inject(DIIdentifier.EventsService) private eventsService: EventsService
    ) {

        eventsService.onLobbyLeft.on(this.stop.bind(this));
        eventsService.onCountdownStarted.on(this.onCountdownStarted.bind(this));
        eventsService.onRoundStarted.on(this.onRoundStarted.bind(this));

        alt.onServer(ToClientEvent.SpectatorReattachCam, this.onSpectatorReattachCam.bind(this));
        alt.onServer(ToClientEvent.PlayerSpectateMode, this.onPlayerSpectateMode.bind(this));
        alt.onServer(ToClientEvent.SetPlayerToSpectatePlayer, this.onSetPlayerToSpectatePlayerMethod.bind(this));
        alt.onServer(ToClientEvent.StopSpectator, this.onStopSpectator.bind(this));
    }


    start() {
        if (this.binded) {
            return;
        }
        this.binded = true;

        //Todo: Naming correct?
        this.eventsService.onSpawned.emit();

    }

}
