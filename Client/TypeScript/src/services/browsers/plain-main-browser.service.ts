import BrowserServiceBase from "./browser-service-base";
import EventsService from "../events/events.service";
import { plainHtmlMainBrowserPath } from "../../datas/constants";
import { logError } from "../../datas/helper/logging.helper";
import alt from "alt-client";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";


export default class PlainMainBrowserService extends BrowserServiceBase {
    private roundEndReasonShowing: boolean;

    constructor(eventsService: EventsService) {
        super(plainHtmlMainBrowserPath);

        this.createBrowser(true);
         //SetReady(); Only for Angular

        eventsService.onLobbyLeft.on(this.hideRoundEndReason.bind(this));
        eventsService.onMapChanged.on(this.hideRoundEndReason.bind(this));
        eventsService.onRoundStarted.on(this.hideRoundEndReason.bind(this));
        eventsService.onCountdownStarted.on(this.hideRoundEndReason.bind(this));

        alt.onServer(ToClientEvent.LoadOwnMapRatings, this.onLoadOwnMapRatings.bind(this));
        alt.onServer(ToClientEvent.PlayCustomSound, this.playSound.bind(this));

        //Todo Implement that later, find out how
        /*modAPI.Event.PlayerStartTalking.Add(new EventMethodData<PlayerDelegate>(EventHandler_PlayerStartTalking));
        modAPI.Event.PlayerStopTalking.Add(new EventMethodData<PlayerDelegate>(EventHandler_PlayerStopTalking));*/
    }

    public addKillMessage(msg: string) {
        this.execute("d", msg);
    }

    public hideRoundEndReason() {
        try {
            if (!this.roundEndReasonShowing)
                return;
            this.execute("g");
            this.roundEndReasonShowing = false;
        }
        catch (ex)
        {
            logError(ex, "HideRoundEndReason failed");
        }
    }

    public onLoadOwnMapRatings(datajson: string) {
        this.execute("h", datajson);
    }

    public playHitsound() {
        this.execute("b");
    }

    public playSound(soundname: string) {
        this.execute("a", soundname);
    }

    public sendAlert(msg: string) {
        this.execute("i", msg);
    }

    public showBloodscreen() {
        this.execute("c");
    }

    public showRoundEndReason(reason: string, mapId: number) {
        this.roundEndReasonShowing = true;
        this.execute("j", reason, mapId);
    }

    public startBombTick(msToDetonate: number, startAtMs: number) {
        this.execute("k", msToDetonate, startAtMs);
    }

    public startPlayerTalking(name: string) {
        this.execute("e", name);
    }

    public stopBombTick() {
        this.execute("l");
    }

    public stopPlayerTalking(name: string) {
        this.execute("f", name);
    }

    /*private eventHandler_PlayerStartTalking(IPlayer player) {
        StartPlayerTalking(player.Name);
    }

    private eventHandler_PlayerStopTalking(IPlayer player) {
        StopPlayerTalking(player.Name);
    }*/

    //Todo Implement that in Angular, remove it from main browser
    /*private onBrowserSendMapRatingMethod(mapId: number, rating: number) {
        this.remoteEventsSender.send(ToServerEvent.SendMapRating, mapId, rating);
    }*/
}
