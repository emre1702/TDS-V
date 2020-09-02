import { injectable, inject } from "inversify";
import Camera from "../../entities/cameras/camera.entity";
import alt from "alt-client";
import SpectateService from "./spectate.service";
import game from "natives";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import EventsService from "../events/events.service";
import RemoteEventsSender from "../events/remote-events-sender.service";
import BindsService from "../input/binds.service";

@injectable()
export default class CamerasService {
    activeCamera: Camera;
    focusAtPos: alt.Vector3;

    betweenRoundsCam: Camera;
    freeCam: Camera;
    spectateCam: Camera;

    private spectateService: SpectateService;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.CamerasService) camerasService: CamerasService,
        @inject(DIIdentifier.BindsService) bindsService: BindsService,
        @inject(DIIdentifier.RemoteEventsSender) remoteEventsSender: RemoteEventsSender,
        @inject(DIIdentifier.Factory_Camera) private cameraFactory: (name: string) => Camera
    ) {
        this.spectateService = new SpectateService(eventsService, camerasService, bindsService, remoteEventsSender);

        game.renderScriptCams(false, false, 0, true, false, 0);
        game.destroyAllCams(false);
        this.createInitialCams();
    }

    getCurrentCamPos(): alt.Vector3 {
        if (this.activeCamera) {
            return this.activeCamera.position;
        } else {
            return game.getGameplayCamCoord() as alt.Vector3;
        }
    }

    getCurrentCamRot(): alt.Vector3 {
        if (this.activeCamera) {
            return this.activeCamera.rotation;
        } else {
            return game.getGameplayCamRot(2) as alt.Vector3;
        }
    }

    setFocusArea(pos: alt.Vector3) {
        if (!this.focusAtPos || this.focusAtPos.distanceTo(pos) >= 50)
        {
            game.setFocusPosAndVel(pos.x, pos.y, pos.z, 0, 0, 0);
            this.focusAtPos = pos;
        }
    }

    renderBack(ease: boolean = false, easeTime: number = 0) {
        this.spectateService.checkSpectatingEntityExists();
        if (this.spectateService.spectatingEntity) {
            this.renderBackToSpectate(ease, easeTime);
        } else {
            this.renderBackToGameplay(ease, easeTime);
        }
    }

    removeFocusArea() {
        game.clearFocus();
        this.focusAtPos = undefined;
    }

    private createInitialCams() {
        this.betweenRoundsCam = this.cameraFactory("BetweenRoundsCam");
        this.spectateCam = this.cameraFactory("BetweenRoundsCam");
        //Freecam is not created here - it's created on demand
    }

    private renderBackToSpectate(ease: boolean = false, easeTime: number = 0) {
        game.setFocusEntity(this.spectateService.spectatingEntity.scriptID);
        this.spectateCam.spectate(this.spectateService.spectatingEntity);
        this.spectateCam.setActive();
        this.spectateCam.render(true, ease, easeTime);
    }

    private renderBackToGameplay(ease: boolean = false, easeTime: number = 0) {
        if (this.activeCamera) {
            this.activeCamera.setInactive();
            this.activeCamera = undefined;
        }
        this.removeFocusArea();
        game.renderScriptCams(false, ease, easeTime, true, false, 0);
    }
}
