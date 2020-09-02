import EventsService from "../events/events.service";
import alt from "alt-client";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";
import CamerasService from "./cameras.service";
import { defaultSpectatePlayerChangeEaseTime } from "../../datas/constants";
import BindsService from "../input/binds.service";
import Key from "../../datas/enums/input/key.enum";
import RemoteEventsSender from "../events/remote-events-sender.service";
import ToServerEvent from "../../datas/enums/events/to-server-event.enum";

export default class SpectateService {

    isSpectator: boolean;

    private _spectatingEntity: alt.Entity;
    get spectatingEntity(): alt.Entity {
        return this._spectatingEntity;
    }
    set spectatingEntity(entity: alt.Entity) {
        if (this._spectatingEntity === entity) {
            return;
        }
        this._spectatingEntity = entity;

        if (entity) {
            this.eventsService.onSpawned.emit();
            this.camerasService.spectateCam.spectate(entity);
        } else {
            this.camerasService.spectateCam.detach();
        }

        this.camerasService.spectateCam.render(true, true, defaultSpectatePlayerChangeEaseTime);
    }
    
    private binded: boolean;

    constructor(
        private eventsService: EventsService,
        private camerasService: CamerasService,
        private bindsService: BindsService,
        private remoteEventsSender: RemoteEventsSender
    ) {

        eventsService.onLobbyLeft.on(this.stop.bind(this));
        eventsService.onCountdownStarted.on(this.onCountdownStarted.bind(this));
        eventsService.onRoundStarted.on(this.onRoundStarted.bind(this));

        alt.onServer(ToClientEvent.SpectatorReattachCam, this.onSpectatorReattachCam.bind(this));
        alt.onServer(ToClientEvent.PlayerSpectateMode, this.onPlayerSpectateMode.bind(this));
        alt.onServer(ToClientEvent.SetPlayerToSpectatePlayer, (entity: alt.Entity) => this.spectatingEntity = entity);
        alt.onServer(ToClientEvent.StopSpectator, this.stop.bind(this));
    }


    start() {
        if (this.binded) {
            return;
        }
        this.binded = true;

        this.eventsService.onSpawned.emit();
        this.camerasService.spectateCam.isActive = true;
        this.camerasService.spectateCam.render(true, true, defaultSpectatePlayerChangeEaseTime);

        this.bindsService.addKey(Key.RightArrow, this.next.bind(this));
        this.bindsService.addKey(Key.D, this.next.bind(this));
        this.bindsService.addKey(Key.LeftArrow, this.previous.bind(this));
        this.bindsService.addKey(Key.A, this.previous.bind(this));
    }

    stop() {
        this._spectatingEntity = undefined;
        if (!this.binded) {
            return;
        }
        this.binded = false;

        this.camerasService.spectateCam.setInactive();

        this.bindsService.removeKey(Key.RightArrow, this.next.bind(this));
        this.bindsService.removeKey(Key.D, this.next.bind(this));
        this.bindsService.removeKey(Key.LeftArrow, this.previous.bind(this));
        this.bindsService.removeKey(Key.A, this.previous.bind(this));
    }

    checkSpectatingEntityExists() {
        if (!this._spectatingEntity) {
            return;
        }
        if (!this._spectatingEntity.exists) {
            this.spectatingEntity = undefined;
            this.camerasService.spectateCam.spectatingEntity = undefined;
        }
    }

    private next() {
        this.remoteEventsSender.send(ToServerEvent.SpectateNext, true);
    }

    private previous() {
        this.remoteEventsSender.send(ToServerEvent.SpectateNext, false);
    }

    private onCountdownStarted(data: { isSpectator: boolean }) {
        if (data.isSpectator) {
            this.start();
        }
    }

    private onRoundStarted(data: { isSpectator: boolean }) {
        this.isSpectator = data.isSpectator;
    }

    private onSpectatorReattachCam() {
        if (this.spectatingEntity) {
            this.camerasService.spectateCam.spectate(this.spectatingEntity);
        }
    }

    private onPlayerSpectateMode() {
        this.isSpectator = true;
        this.start();
    }

}
