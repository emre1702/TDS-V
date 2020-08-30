import EventsService from "../events/events.service";
import { inject, injectable } from "inversify";
import RemoteEventsSender from "../events/remote-events-sender.service";
import AngularBrowserService from "./angular-browser.service";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import PlainMainBrowserService from "./plain-main-browser.service";
import { on } from "alt-client";
import FromBrowserEvent from "../../datas/enums/events/from-browser-event.enum";
import RegisterLoginBrowserService from "./register-login-browser.service";

@injectable()
export default class BrowsersService {

    angular: AngularBrowserService;
    mapCreatorObjectChoice: MapCreatorObjectChoiceBrowserService;
    mapCreatorVehicleChoice: MapCreatorVehicleChoiceBrowserService;
    plainMain: PlainMainBrowserService;
    registerLogin: RegisterLoginBrowserService;

    inInput: boolean;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.RemoteEventsSender) remoteEventsSender: RemoteEventsSender
    ) {
        this.angular = new AngularBrowserService(eventsService);


        this.plainMain = new PlainMainBrowserService(eventsService);
        this.registerLogin = new RegisterLoginBrowserService(eventsService);

        //Todo: Does alt.on even work for browser? Or do I need to use browser.on?
        on(FromBrowserEvent.InputStarted, () => this.inInput = true);
        on(FromBrowserEvent.InputStarted, () => this.inInput = false);
    }
}
