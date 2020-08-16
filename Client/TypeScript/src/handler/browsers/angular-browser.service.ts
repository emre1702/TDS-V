import EventsService from "../events/events.service";
import BrowserServiceBase from "./browser-service-base";

export default class AngularBrowserService extends BrowserServiceBase {

    constructor(
        @inject(EventsService) eventsService: EventsService
    ) {

    }
}
