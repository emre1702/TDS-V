import EventsService from "../events/events.service";
import { inject, injectable } from "inversify";
import RemoteEventsSender from "../events/remote-events-sender.service";
import AngularBrowserService from "./angular-browser.service";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";

@injectable()
export default class BrowsersService {

    angular: AngularBrowserService;
    mapCreatorObjectChoice: MapCreatorObjectChoiceBrowserService;
    mapCreatorVehicleChoice: MapCreatorVehicleChoiceBrowserService;
    plainMain: PlainMainBrowserService;
    inInput: boolean;

    constructor(
        @inject(DIIdentifier.EventsService) eventsService: EventsService,
        @inject(DIIdentifier.RemoteEventsSender) remoteEventsSender: RemoteEventsSender
    ) {
        this.angular = new AngularBrowserService(eventsService);

    }
}
