import EventsService from "../events/events.service";
import { inject } from "inversify";
import RemoteEventsSender from "../events/remote-events-sender.service";
import AngularBrowserService from "./angular-browser.service";

export default class BrowsersService {

    angular: AngularBrowserService;
    mapCreatorObjectChoice: MapCreatorObjectChoiceBrowserService;
    mapCreatorVehicleChoice: MapCreatorVehicleChoiceBrowserService;
    plainMain: PlainMainBrowserService;
    inInput: boolean;

    constructor(
        @inject(EventsService) eventsService: EventsService,
        @inject(RemoteEventsSender) remoteEventsSender: RemoteEventsSender
    ) {
        this.angular = new AngularBrowserService(eventsService);

    }
}
