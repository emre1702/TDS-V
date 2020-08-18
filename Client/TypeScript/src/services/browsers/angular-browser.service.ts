import EventsService from "../events/events.service";
import BrowserServiceBase from "./browser-service-base";
import { angularMainBrowserPath } from "../../datas/constants";
import { inject } from "inversify";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";
import { onServer } from "alt-client";
import ToClientEvent from "../../datas/enums/events/to-client-event.enum";
import FromBrowserEvent from "../../datas/enums/events/from-browser-event.enum";
import Utils from "../../datas/utils";

export default class AngularBrowserService extends BrowserServiceBase {

    constructor(
        @inject(EventsService) private eventsService: EventsService
    ) {
        super(angularMainBrowserPath);

        eventsService.onInFightStatusChanged.on((inFight) => this.execute(ToBrowserEvent.ToggleRoundStats, inFight));
        eventsService.onLobbyLeft.on((_) => this.execute(ToBrowserEvent.ResetMapVoting));
        eventsService.onRoundEnded.on((_) => this.execute(ToBrowserEvent.ResetMapVoting));
        eventsService.onAngularCooldown.on(() => this.execute(ToBrowserEvent.ShowCooldown));
        eventsService.onChatInputToggled.on((activated) => this.execute(ToBrowserEvent.ToggleChatOpened, activated));

        this.browser.on(FromBrowserEvent.GetHashedPassword, this.onGetHashedPassword); 

       


    }



    private onGetHashedPassword(password: string) {
        const hashedPassword = Utils.hashPasswordClient(pw);
        this.execute(FromBrowserEvent.GetHashedPassword, hashedPassword);
    }

}
